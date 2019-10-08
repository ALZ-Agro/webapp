(function () {
    'use strict';

    angular
        .module('provider-module')
        .controller('ImportDialogController', ImportDialogController);

    ImportDialogController.$inject = ['$scope', '$rootScope', '$mdDialog', 'apiService', 'notificationService', '$http', '$sce', 'Papa', 'Blob'];

    function ImportDialogController($scope, $rootScope, $mdDialog, apiService, notificationService, $http, $sce, Papa, Blob) {
        var fileName = "";
        $scope.status = "idle";
        $scope.url = "";
        $scope.files = [];
        $scope.from = "file";


        $scope.onSourceChanged = function (selected) {
            $scope.from = selected;
        }

       $scope.onXlsRead = parseSheet;

        $scope.readFile = function ($files) {
            $scope.status = "uploading";

            var extension = $files[0].name.substr($files[0].name.lastIndexOf(".") + 1, $files[0].name.length);

            if (!extension) {
                notificationService.displayError("La URL no pertenece a un archivo o este no tiene un formato adecuado para la importación");
            }
            else {
                extension = extension.toLowerCase();
                switch (extension) {
                    case 'csv':
                        var config = {
                            header: true,
                            dynamicTyping: true,
                            encoding: "ISO-8859-1"
                        };
                        fileName = $files[0].name;
                        Papa.parse($files[0], config).then(function (result) {
                            $scope.status = "idle";
                            $mdDialog.hide({ data: result.data, fileName: fileName });
                        })
                             .catch(function (err) {
                                 $scope.status = "idle";
                                 notificationService.displayError("Ocurrió un error al leer el archivo o este no posee datos cargados");
                             });
                        break;
                    default:
                        //notificationService.displayError("Ocurrió un error al leer el archivo o este no posee datos cargados");
                        fileName = $files[0].name;
                        parseSheet($files[0]);
                        break;

                }


            }
        }

            function parseSheet(file) {
                var reader = new FileReader();

                reader.onload = function (e) {

                    var data = e.target.result;

                    var workbook = XLSX.read(data, { type: 'binary' });

                    var first_sheet_name = workbook.SheetNames[0];

                    var dataObjects = XLSX.utils.sheet_to_json(workbook.Sheets[first_sheet_name]);

                    if (dataObjects.length > 0) {
                        $mdDialog.hide({ data: dataObjects, fileName: fileName });

                    } else {
                        notificationService.displayError("Ocurrió un error al leer el archivo o este no posee datos cargados");
                    }

                }

                reader.onerror = function (ex) {

                }

                reader.readAsBinaryString(file);
        }
               
               $scope.onError = function(error) {
                   notificationService.displayError("Ocurrió un error al leer el archivo.");
               }
              

               $scope.cancel = function () {
                   $mdDialog.cancel();
               }
    }

})();