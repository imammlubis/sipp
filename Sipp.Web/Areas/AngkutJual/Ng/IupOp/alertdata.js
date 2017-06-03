var app = angular.module('alert', []);
app.controller('alert-controller', function ($scope, $http, $interval, $timeout) {
    $scope.isBusy = false;

    $scope.initWarningModel = function () {
        $scope.warningModel = {
            ID: '',
            LetterNumber: '',
            LetterDate: '',
            SkFile: '',
            WarningDuration: '',
            ObligationsShould: '',
            ObligationsYet: '',
            ObligationsAlready: '',
            KeteranganSkHabis: '',
            AdditionalInfo: '',
            WarningType: '',
            EndDate: '',
            CompanyID: '',
            CompanyName: '',
            DestinationEmail: '',
            DestinationMobileNo: ''
        }
    }
    $scope.initWarningModel();

    $scope.getData = function (id) {
        $http.get('/angkutjual/iupopangkutjual/findWarning/' + id)
        .success(function (data) {
            $scope.warningModel.DestinationEmail = data.email;
            $scope.warningModel.DestinationMobileNo = data.mobile;
        })
        .finally(function () {

        })
    }
    $scope.getData($('#ID').val());
    $scope.sendSkHabis = function () {
        debugger;
        $http.post('/AngkutJual/IupOpAngkutJual/SendSkHabis/', {
            CompanyID: $('#ID').val(),//$scope.warningModel.CompanyID,
            CompanyName: $('#NmPerusahaan').val(),
            DestinationEmail: $scope.warningModel.DestinationEmail,
            DestinationMobileNo: $scope.warningModel.DestinationMobileNo,
            Keterangan: $scope.warningModel.KeteranganSkHabis
        })
        .success(function (data) {
            $scope.initWarningModel();
            alert('Sukses Sending Message');
        })
        .finally(function () {
            $scope.getData($('#ID').val());
            //App.unblockUI();
        })
    }
    $scope.buttonBatal = function () {
        location.href = '/angkutjual/iupopangkutjuallist';
    }

    //$scope.warningModel.LetterDate = $('#tglSurat').datepicker({
    //    format: 'dd-M-yyyy',
    //    autoclose: true,
    //});

    $scope.sendAlert = function () {
        /*
         ID = Guid.NewGuid().ToString(),
                LetterNumber = model.LetterNumber,
                LetterDate = model.LetterDate,
                WarningDuration = model.WarningDuration,
                ObligationsAlready = model.ObligationsAlready,
                ObligationsShould = model.ObligationsShould,
                ObligationsYet = model.ObligationsYet,
                AdditionalInfo = model.AdditionalInfo,
                WarningType = model.WarningType,
                CompanyID = model.CompanyID,
                DestinationEmail = model.DestinationEmail,
                DestinationMobileNo = model.DestinationMobileNo
        */
        debugger;
        $scope.isBusy = true;
        $scope.warningModel.CompanyID = $('#ID').val();
        $scope.warningModel.CompanyName = $('#NmPerusahaan').val();
        //App.blockUI({ message: 'sending notification..' });
        
        $http.post('/AngkutJual/IupOpAngkutJual/SaveWarningLetter/', {
            LetterNumber: $scope.warningModel.LetterNumber,
            LetterDate: $('#tglSurat').val(),//$scope.warningModel.LetterDate,
            SkEndDate: $('#tglEndSurat').val(),
            WarningDuration: $('#hari').val(),//$scope.warningModel.WarningDuration,
            ObligationsAlready: $scope.warningModel.ObligationsAlready,
            ObligationsShould: $scope.warningModel.ObligationsShould,
            ObligationsYet: $scope.warningModel.ObligationsYet,
            AdditionalInfo: $scope.warningModel.AdditionalInfo,
            WarningType: $scope.warningModel.WarningType,
            CompanyID: $scope.warningModel.CompanyID,
            CompanyName: $scope.warningModel.CompanyName,
            DestinationEmail: $scope.warningModel.DestinationEmail,
            DestinationMobileNo: $scope.warningModel.DestinationMobileNo
        })
        .success(function (data) {
            debugger;
            location.href = '/angkutjual/iupopangkutjuallist/'
        })
        .finally(function () {
            //App.unblockUI();
        })


        //$http.get('/AngkutJual/IupOpAngkutJual/SendEmail')
        //.success(function(){
        //    alert('sent')
        //})
        //.finally(function () {

        //    $http.get('/AngkutJual/IupOpAngkutJual/SendSms')
        //     .success(function () {
        //         alert('sent')
        //     });
        //})



    }

});