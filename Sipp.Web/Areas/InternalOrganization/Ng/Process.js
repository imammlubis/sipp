var app = angular.module('internal', []);
app.filter("mydate", function () {
    return function (x) {
        if (x != null)
            return new Date(parseInt(x.substr(6)));

    };
});
app.controller('internal-controller', function ($scope, $http, $interval, $timeout) {

    $('.date-picker').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $(".js-example-basic-single").select2();
    $scope.initBillingModel = function () {
        $scope.billingModel = {
            id: '',
            nama: '',
            pemeriksa:'',
            periodepemeriksaan: '',
            tahunpenagihan: '',
            nosurat: '',
            tanggaltagihan: '',
            tipetagihan: '',        
            nominal: ''
        }
    }
    $scope.initBillingModel();


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
 })