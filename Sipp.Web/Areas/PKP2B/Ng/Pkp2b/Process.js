

var app = angular.module('pkp2b', []);

app.controller('pkp2b-controller', function ($scope, $http, $interval, $timeout) {
    
    $('#tanggalKontrak').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#tanggalBerakhir').datepicker({ format: "dd/mm/yyyy", autoclose: true, })

    $scope.initCompanyModel = function () {
        $scope.companyModel = {
            id: '',
            nama: '',
            npwp: '',            
            telp: '',
            fax: '',
            alamat: '',            
            email: '',
            website: '',
            namacp: '',
            hpcp: '',
        }
    }
    $scope.initCompanyModel();

    $scope.initDataPkp2bModel = function () {
        $scope.dataPkp2bModel = {
            id: '',
            generasi: '',
            kodeWilayah: '',
            noKontrak: '',
            tanggalKontrak: '',
            luasWilayahAwal: '',
            luasWilayahDipertahankan: '',
            provinsi: '',
            perizinan: '',
            kabupaten: '',
            tahapanAkhir: '',
            tanggalBerakhir: '',
            jangkaWaktu: ''
        }
    }
    $scope.initDataPkp2bModel();
    $scope.companyModel.hpcp = '+62';

    $scope.SaveGeneralInformation = function () {
        if ($scope.userForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateServiceGeneralInformation', {
            ID: $scope.idx,
            Name: $scope.companyModel.nama.toUpperCase(),
            NPWP: $('#npwp').val(),//$scope.companyModel.npwp,//$('#NpWp').val(),
            Telp: $scope.companyModel.telp,
            Fax: $scope.companyModel.fax,
            Alamat: $scope.companyModel.alamat,
            Email: $scope.companyModel.email,
            Website: $scope.companyModel.website,
            CpNama: $scope.companyModel.namacp.toUpperCase(),
            CpHp: $scope.companyModel.hpcp,
            Generasi : $scope.dataPkp2bModel.generasi,
            KodeWilayah: $scope.dataPkp2bModel.kodeWilayah,
            NoKontrak: $scope.dataPkp2bModel.noKontrak,
            TanggalKontrak: $('#tanggalKontrak').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.dataPkp2bModel.tanggalKontrak,
            LuasWilayahAwal: $scope.dataPkp2bModel.luasWilayahAwal,
            LuasWilayahDipertahankan: $scope.dataPkp2bModel.luasWilayahDipertahankan,
            Provinsi: $scope.dataPkp2bModel.provinsi.toUpperCase(),
            Perizinan: $scope.dataPkp2bModel.perizinan,
            Kabupaten: $scope.dataPkp2bModel.kabupaten.toUpperCase(),
            TahapanAkhir: $scope.dataPkp2bModel.tahapanAkhir,
            TanggalBerakhir: $('#tanggalBerakhir').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.dataPkp2bModel.tanggalBerakhir,
            JangkaWaktu: $scope.dataPkp2bModel.jangkaWaktu,
        })
        .success(function (data) {
            //swal("Sukses!", "Data tersimpan", "success");
            toastr["success"]("Data Berhasil Disimpan..");
            window.location.href = '/Pkp2b/MemasukanData/Edit/' + data;
        })
    }
    //$scope.SaveGeneralInformation = function () {
    //    debugger
    //    if ($scope.userForm.$invalid) { return; }
    //    else
    //        alert('our form is amazing');
        
    //}
    $scope.loadDetails = function () {

    }
 })