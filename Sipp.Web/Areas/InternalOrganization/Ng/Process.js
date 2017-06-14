


var app = angular.module('internal', []);
app.filter("mydate", function () {
    return function (x) {
        if (x != null)
            return new Date(parseInt(x.substr(6)));
    };
});

app.controller('internal-controller', function ($scope, $http, $interval, $timeout) {
    $scope.idx = $('#idx').val();
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
    $scope.initCompanyModel = function () {
        $scope.companyModel = {
            id: '',
            nama: '',
            email: '',
            provinsi:'',
            legaltype:''
        }
    }
    $scope.initCompanyModel();

    $scope.initCompany2Model = function () {
        $scope.company2Model = {
            id2: '',
            nama2: '',
            email2: '',
            provinsi2: '',
            legaltype2: ''
        }
    }
    $scope.initCompany2Model();

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
            $('#myModal').modal('hide');
        })
        .finally(function () {
            setInterval(function () {
                window.location.href = '/InternalOrganization/Billing/';
            }, 1000)            
        })
    }
    $scope.CreateCompany = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/internalorganization/billing/CreateCompany', {
            ID: '-',
            Name: $scope.companyModel.nama,
            Province: $scope.companyModel.provinsi,
            Email: $scope.companyModel.email,
            LegalType: $scope.companyModel.legaltype
        })
        .success(function (data) {
            toastr["success"]("Data Berhasil Disimpan..");
            $('#myModal').modal('hide');
            $('#nama').val('');
            $('#provinsi').val('');
            $('#email').val('');
            $('#tipe').val('');
        })
        .finally(function () {
            $scope.initCompanyModel();
            $("#grid").data("kendoGrid").dataSource.read();
            $("#grid").data("kendoGrid").refresh();
            //setInterval(function () {
            //    window.location.href = '/InternalOrganization/Billing/';
            //}, 1000)
        })
    }

    //$scope.editCompany = function (id) {
    //    debugger;
    //    $http.get('/internalorganization/billing/FindDataCompany/' + id)
    //    .success(function (data) {
    //        debugger;
    //        $scope.persetujuanModel.id = data.ID,
    //        $scope.persetujuanModel.JenisPersetujuan = data.JenisPersetujuan,
    //        $scope.persetujuanModel.noSK = data.NoSk,
    //        $scope.persetujuanModel.terbukti = data.Terbukti
    //        $scope.persetujuanModel.file = data.File,
    //        $("#tanggalSuratPersetujuan").datepicker("update", data.TanggalSk);
    //        //$('#static3').modal('show');
    //        $scope.isEditDataPersetujuan = true;
    //    })
    //}
    
    $scope.EditCompany = function () {
        $scope.initCompanyModel();
        debugger;
        toastr["info"]("Menyimpan Data..");
        $http.post('/internalorganization/billing/EditCompany', {
            Id: $('#idx').val(),
            Name: $('#nama2').val(),
            Province: $('#provinsi2').val(),
            Email: $('#email2').val(),
            LegalType: $('#tipe2').val()
        })
        .success(function (data) {
            toastr["success"]("Data Berhasil Disimpan..");
            
            $('#nama2').val('');
            $('#provinsi2').val('');
            $('#email2').val('');
            $('#tipe2').val('');
        })
        .finally(function () {
            $('#myModalEdit').modal('hide');
            $("#grid").data("kendoGrid").dataSource.read();
            $("#grid").data("kendoGrid").refresh();
            //setInterval(function () {
            //    window.location.href = '/InternalOrganization/Billing/';
            //}, 1000)
        })
    }
 })