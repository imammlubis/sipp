var app = angular.module('testing', []);


app.controller('testing-controller', function ($scope, $http, $interval, $timeout) {
    $scope.isAddCompany = false;

    $scope.initCompanyModel = function () {
        $scope.companyModel = {
            id: '',
            name: '',
            npwp: ''
        }
    }

    $scope.addCompany = function () {
        $scope.isAddCompany = true;
    }

    $scope.saveCompany = function () {
        $http.post('/Organization/Companies/CreateService', {
            Name: $scope.companyModel.name,
            NPWP : $scope.companyModel.npwp
        })
        .success(function (data) {
          
            $scope.loadCompany();
            $scope.initCompanyModel();
            $scope.isAddCompany = false;
        })
        .error(function () { })
        .finally(function () { })

    }

    $scope.loadCompany = function () {
        $http.get('/organization/companies/listcompanyfortesting')
        .success(function (data) {
            $scope.listCompany = data.data;

        })
    };
    
    $scope.loadCompany();

})