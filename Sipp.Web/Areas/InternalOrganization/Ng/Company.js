var app = angular.module('company', []);
app.filter("mydate", function () {
    return function (x) {
        if (x != null)
            return new Date(parseInt(x.substr(6)));

    };
});
app.controller('company-controller', function ($scope, $http, $interval, $timeout) {

    $('.date-picker').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    
    $scope.initCreditModel = function () {
        $scope.creditModel = {
            id: '',
            nominal: '',
            bukti:'' ,
            jumlahPiutang:''
        }
    }
    $scope.initCreditModel();
    $scope.LoadListCompany = function () {
        $http.get('/InternalOrganization/Billing/LoadListCompany')
        .success(function (data) {
            $scope.listcompanydata = data;
        })
    };
    $scope.LoadListCompany();

    $scope.CreateDetailKronologiTahapan = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/internalorganization/billing/CreateFirstBill', {
            ID: '-',
            CompanyID: $('#companyId').val(),
            Evaluator: $scope.billingModel.pemeriksa,
            YearOfCheckingPeriod:$scope.billingModel.periodepemeriksaan,
            YearOfBillingPeriod:$scope.billingModel.tahunpenagihan,
            FirstBillingNo:$scope.billingModel.nosurat,
            FirstBillingDate: $('#tanggaltagihan1').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            FirstAmount: $scope.billingModel.nominal,
            BillingType: $scope.billingModel.tipetagihan
        })
        .success(function (data) {
            toastr["success"]("Data Berhasil Disimpan..");            
        })
        .finally(function () {
            setInterval(function () {
                window.location.href = '/InternalOrganization/Billing/';
            }, 1000)            
        })
    }

    $scope.CreateTransaction = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/internalorganization/Companies/CreateTransaction', {
            ID: '-',
            Amount: $('#nominal').val(),
            //FileValidation: '',
            ObjectionInformation: $('#keterangan').val()
        })
        .success(function (data) {
            toastr["success"]("Data Berhasil Disimpan..");
        })
        .finally(function () {
          
        })
    }
 })