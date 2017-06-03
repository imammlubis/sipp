var app = angular.module('notification', []);
app.controller('notification-controller', function ($scope, $http, $interval, $timeout) {

    //alert('cuk')

    $scope.isBusy = false;

    $scope.initModel = function () {

        $scope.notificationModel = {
            CompanyName:'',            
            DestinationEmail: '',
            DestinationMobileNo:'',
            AdditionalInfo: '',            
        }
      
    }

    $scope.initModel();

    $scope.sendNotification = function (resp) {


        $scope.isBusy = true;


       //App.blockUI({ message: 'sending notification..' });

        $http.post('/AngkutJual/RKABRealizations/SendNotification/', {
            resp : resp,
            CompanyName: $scope.notificationModel.CompanyName,
            DestinationEmail: $scope.notificationModel.DestinationEmail,
            DestinationMobileNo: $scope.notificationModel.DestinationMobileNo,
            AdditionalInfo: $scope.notificationModel.AdditionalInfo
        })
        .success(function (data) {
            location.href = '/angkutjual/RKABRealizations/'
        })
        .finally(function () {
            //App.unblockUI();
        })



    
    }

});