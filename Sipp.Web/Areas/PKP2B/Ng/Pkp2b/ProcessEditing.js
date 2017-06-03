var app = angular.module('pkp2b', []);

app.filter('formatDate', function (dateFilter) {
    var formattedDate = '';
    return function (dt) {
        return new Date(parseInt(dt.substr(6)));
    }
});

app.controller('pkp2b-controller', function ($scope, $http, $interval, $timeout) {

    $("#jangkaWaktu").on('change', function () {
        if ($('#tanggalKontrak').data('datepicker').getFormattedDate('dd/mm/yyyy') == '') {
            $('#tanggalBerakhir').val('');
            document.getElementsById("tanggalKontrak").focus();
            $('#tanggalBerakhir').datepicker('update', '')
            alert('Tolong Isi Tanggal Kontrak');
        }
        else {
            var length = $('#jangkaWaktu').val();
            var dateMin = $('#tanggalKontrak').datepicker("getDate");
            var rMax = new Date(dateMin.getFullYear() + parseInt(length), dateMin.getMonth(), dateMin.getDate() - 1);
            $('#tanggalBerakhir').datepicker({ format: "dd/mm/yyyy" });
            $('#tanggalBerakhir').datepicker('update', rMax.toLocaleDateString());
        }
    });


    $('#tanggalKontrak').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    //$('#tanggalBerakhir').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#tanggalAkta').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#tanggalPk').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#tanggalSuratPersetujuan').datepicker({ format: "dd/mm/yyyy", autoclose: true, })

    $('#tanggalSkKtDetail').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#tanggalSkKt').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#mulaiKtDetail').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#mulaiKt').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#sdKtDetail').datepicker({ format: "dd/mm/yyyy", autoclose: true, })
    $('#sdKt').datepicker({ format: "dd/mm/yyyy", autoclose: true, })

    //ddl get value//
    $scope.changeddl = function () {
        if ($('#statuswnbhshareholder').val() == 'WNI')
            $scope.shareholderModel.negara = 'INDONESIA'
        else
            $scope.shareholderModel.negara = ''
    }
    $scope.changeddl2 = function () {
        if ($('#direksiId').val() == 'WNI')
            $scope.DirectorModel.negara = 'INDONESIA'
        else
            $scope.DirectorModel.negara = ''
    }
    $scope.changeddl3 = function () {
        if ($('#komisarisId').val() == 'WNI')
            $scope.CommisionerModel.negara = 'INDONESIA'
        else
            $scope.CommisionerModel.negara = ''
    }

    //tanggalSuratPersetujuan
    $scope.idx = $('#idx').val();
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

    $scope.modelUpload = {
        id: '',
        title: '',
        moduleType: ''
    };
//==============================================UPLOAD DATA================================================//
    function onSelect(e) {
        console.log("Select :: " + getFileInfo(e));
    }
    function onUpload(e) {
        console.log("Upload :: " + getFileInfo(e));
        e.data = {
            id: $scope.modelUpload.id,
            moduleType: $scope.modelUpload.moduleType
        }
        $('#modalUploadFile').modal('hide');
        //$scope.loadSkAwalIupOp();
    }

    function onSuccess(e) {
        console.log("Success (" + e.operation + ") :: " + getFileInfo(e));
    }

    function onError(e) {
        //  console.log("Error (" + e.operation + ") :: " + getFileInfo(e));
    }

    function onComplete(e) {
        console.log("Complete");
        if ($scope.modelUpload.moduleType == "1") {
            $scope.loadAkta();
        }
        if ($scope.modelUpload.moduleType == "2") {
            $scope.loadKronologiTahapan();
        }
        else if ($scope.modelUpload.moduleType == "3") {
            $scope.loadDetailKronologiTahapan();
        }
        else if ($scope.modelUpload.moduleType == "4") {
            $scope.loadPersetujuan();
        }
    }
    function onCancel(e) {
        console.log("Cancel :: " + getFileInfo(e));
    }
    function onRemove(e) {
        console.log("Remove :: " + getFileInfo(e));
    }
    function onProgress(e) {
        console.log("Upload progress :: " + e.percentComplete + "% :: " + getFileInfo(e));
    }
    function getFileInfo(e) {
        return $.map(e.files, function (file) {
            var info = file.name;
            // File size is not available in all browsers
            if (file.size > 0) {
                info += " (" + Math.ceil(file.size / 1024) + " KB)";
            }
            return info;
        }).join(", ");
    }
    //upload file
    $("#files").kendoUpload({
        multiple: false,
        async: {
            saveUrl: "../save",
            removeUrl: "../remove",
            autoUpload: true
        },
        cancel: onCancel,
        complete: onComplete,
        error: onError,
        progress: onProgress,
        remove: onRemove,
        select: onSelect,
        success: onSuccess,
        upload: onUpload
    });

    $scope.uploadFile = function () {
        $('#modalUploadFile').modal('show');
    }
//==============================================UPLOAD DATA================================================//

    //Model Data Akta
    $scope.initAktaModel = function () {
        $scope.aktaModel = {
            id: '',
            judulAkta: '',
            noAkta: '',
            tanggalAkta: '',
            noPk: '',
            tanggalPk: '',
            filePk:''
        }
    }
    $scope.initAktaModel();

    $scope.initBiayaEksplorasiModel = function(){
        $scope.biayaEksplorasiModel = {
            id:'',
            mataUang: '',
            jumlahUang:'',
            companyId:''
        }
    }
    $scope.initBiayaEksplorasiModel();

    //Model Persetujuan
    $scope.initPersetujuanModel = function () {
        $scope.persetujuanModel = {
            id: '',
            jenisPersetujuan: '',
            noSK: '',
            tanggalSurat: '',
            file: ''
        }
    }
    $scope.initPersetujuanModel();
    $scope.initCommisionerModel = function () {
        $scope.CommisionerModel = {
            ID: '',
            nama: '',
            status: '',
            negara: '',
            keterangan:'',
            CompanyID: '',
            isActive: ''
        }
    }
    $scope.initCommisionerModel();

    $scope.initDirectorModel = function () {
        $scope.DirectorModel = {
            ID: '',
            nama: '',
            status: '',
            CompanyID: '',
            negara: '',
            keterangan: '',
            isActive:''
        }
    }
    $scope.initDirectorModel();

    //Model Kronologi Tahapan
    $scope.initKronologiTahapanModel = function () {
        $scope.kronologiTahapanModel = {
            id: '',
            tahapan: '',
            noSk: '',
            tanggalSk: '',
            mulai: '',
            sampaiDengan: '',
            jangkaWaktu: '',
            file: '',
            keterangan:''
        }
    }
    $scope.initKronologiTahapanModel();

    //Model Detail Kronologi Tahapan
    $scope.initDetailKronologiTahapanModel = function () {
        $scope.detailKronologiTahapanModel = {
            id: '',
            tahapan: '',
            noSk: '',
            tanggalSk: '',
            mulai: '',
            sampaiDengan: '',
            keterangan: '',
            tahapanInduk: '',
            jangkaWaktu: '',
            file: ''
            
        }
    }
    $scope.initDetailKronologiTahapanModel();

    //Model Pemegang Saham
    $scope.initShareholderModel = function () {
        $scope.shareholderModel = {
            id: '',
            nama: '',
            statuswnbh: '',
            negara:'',
            jumlahSaham: '',
            nilaiSaham: '',
            prosentaseSaham:'',
            isActive: ''
        }
    }
    $scope.initShareholderModel();

    //Model Cadangan Batubara
    $scope.initCadanganBatubaraModel = function () {
        $scope.cadanganBatubaraModel = {
            id: '',
            blok: '',
            terkira: '',
            terbukti: '',
            sr:''
        }
    }
    $scope.initCadanganBatubaraModel();

    //Model Kualitas Batubara
    $scope.initKualiatasBatubaraModel = function () {
        $scope.kualitasBatubaraModel = {
            id: '',
            parameter: '',
            blok: '',
            sampleCode: '',
            tm: '',
            im: '',
            ash: '',
            vm: '',
            fc: '',
            ts: '',
            adb: '',
            daf: '',
            ar: ''
        }
    }
    $scope.initKualiatasBatubaraModel();

    //Model Tahap Kegiatan
    $scope.initTahapKegiatanModel = function () {
        $scope.tahapKegiatanModel = {
            id: '',
            tahapKegiatan: '',
            luasDipertahankan: ''
        }
    }
    $scope.initTahapKegiatanModel();

    //Model Tahap Kegiatan Blok
    $scope.initTahapKegiatanBlokModel = function () {
        $scope.tahapKegiatanBlokModel = {
            id: '',
            tahap: '',
            luasDipertahankan: '',
            tahapkegiatanId: ''
        }
    }
    $scope.initTahapKegiatanBlokModel();

    //Model Sumber daya batubara
    $scope.initSumberDayaBatubaraModel = function () {
        $scope.SumberDayaBatubaraModel = {
            id: '',
            blok: '',
            tereka: '',
            terunjuk: '',
            terukur: ''
        }
    }
    $scope.initSumberDayaBatubaraModel();

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
            kabupaten: '',
            tahapanAkhir: '',
            tanggalBerakhir: '',
            perizinan: '',
            jangkaWaktu: ''
        }
    }
    $scope.initDataPkp2bModel();

    //=============================================================== Data Akta ==================================
    $scope.tambahDataAktaClick = function () {
        $scope.initAktaModel();
        $('form[name="dataAktaForm"]')[0].reset();
    }


    $scope.CreateDataAkta = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateDataAkta', {
            ID: '-',
            CompanyID: $scope.idx,
            JudulAkta: $scope.aktaModel.judulAkta.toUpperCase(),
            NoAkta: $scope.aktaModel.noAkta.toUpperCase(),
            TanggalAkta: $('#tanggalAkta').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.aktaModel.tanggalAkta,
            NoPengesahanKemenkumham: $scope.aktaModel.noPk.toUpperCase(),
            TanggalPengesahanKemenkumhan: $('#tanggalPk').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.aktaModel.tanggalPk,
            File: $scope.aktaModel.filePk
        })
        .success(function (data) {
            debugger;
            //swal("Sukses!", "Data tersimpan", "success");
            $('#static').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadAkta();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Data Akta';
            $scope.modelUpload.moduleType = '1'
            $scope.uploadFile();
            
        })
        .finally(function (){
            $scope.initAktaModel();
        })
    }
    $scope.deleteDataAkta = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteDataAkta', { id: id })
            .success(function (data) {
                $scope.loadAkta();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }
    $scope.loadAkta = function () {
        $http.get('/pkp2b/memasukandata/LoadAkta/' + $scope.idx)
        .success(function (data) {
            $scope.dataAkta = data.data;
        })
    }
    $scope.loadAkta();

    $scope.editDataAktaClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindDataAkta/' + id)
        .success(function (data) {
            debugger;
            $scope.aktaModel.id = data.ID,
            $scope.aktaModel.judulAkta = data.JudulAkta,
            $scope.aktaModel.noAkta = data.NoAkta,
            $scope.aktaModel.tanggalAkta = data.TanggalAkta,
            $scope.aktaModel.noPk = data.NoPengesahanKemenkumham,
            $scope.aktaModel.tanggalPk = data.TanggalPengesahanKemenkumhan,
            $scope.aktaModel.filePk = data.File,
            $("#tanggalPk").datepicker("update", data.TanggalPengesahanKemenkumhan);
            $("#tanggalAkta").datepicker("update", data.TanggalAkta);
            $('#static').modal('show');
            $scope.isEditDataAkta = true;
        })
    }    

    $scope.SaveChangesDataAkta = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceDataAkta/', {
            ID: $scope.aktaModel.id,
            CompanyID: $scope.idx,
            JudulAkta: $scope.aktaModel.judulAkta.toUpperCase(),
            NoAkta: $scope.aktaModel.noAkta.toUpperCase(),
            TanggalAkta: $('#tanggalAkta').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.aktaModel.tanggalAkta,
            NoPengesahanKemenkumham: $scope.aktaModel.noPk.toUpperCase(),
            TanggalPengesahanKemenkumhan: $('#tanggalPk').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.aktaModel.tanggalPk,
            File: $scope.aktaModel.filePk,

        })
        .success(function (data) {
            debugger;
            $scope.loadAkta();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Data Akta';
            $scope.modelUpload.moduleType = '1'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initAktaModel();
            $('#static').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditDataAkta = false;
        })
    }
    //=============================================================== Data Akta ==================================

    //=============================================================== Persetujuan ==================================
    $scope.tambahPersetujuanClick = function () {
        $scope.initPersetujuanModel();
        $('form[name="persetujuanForm"]')[0].reset();
    }

    $scope.CreatePersetujuan = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreatePersetujuan', {
            ID: '-',
            CompanyID: $scope.idx,
            JenisPersetujuan: $scope.persetujuanModel.JenisPersetujuan.toUpperCase(),
            NoSk: $scope.persetujuanModel.noSK.toUpperCase(),
            TanggalSk: $('#tanggalSuratPersetujuan').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            File: $scope.persetujuanModel.file
        })
        .success(function (data) {
            $('#static3').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadPersetujuan();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Kronologi Tahapan';
            $scope.modelUpload.moduleType = '4'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initPersetujuanModel();
        })
    }
    $scope.loadPersetujuan = function () {
        $http.get('/pkp2b/memasukandata/LoadPersetujuan/' + $scope.idx)
        .success(function (data) {
            $scope.dataPersetujuan = data.data;
        })
    }
    $scope.loadPersetujuan();
    $scope.deleteDataPersetujuan = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeletePersetujuan', { id: id })
            .success(function (data) {
                $scope.loadPersetujuan();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }


    $scope.editDataPersetujuanClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindDataPersetujuan/' + id)
        .success(function (data) {
            debugger;

            $scope.persetujuanModel.id = data.ID,
            $scope.persetujuanModel.JenisPersetujuan = data.JenisPersetujuan,
            $scope.persetujuanModel.noSK = data.NoSk,
            $scope.persetujuanModel.terbukti = data.Terbukti
            $scope.persetujuanModel.file = data.File,
            $("#tanggalSuratPersetujuan").datepicker("update", data.TanggalSk);

            $('#static3').modal('show');
            $scope.isEditDataPersetujuan = true;
        })
    }
    $scope.SaveChangesDataPersetujuan = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceDataPersetujuan/', {

            ID: $scope.persetujuanModel.id,
            CompanyID: $scope.idx,

            JenisPersetujuan: $scope.persetujuanModel.JenisPersetujuan.toUpperCase(),
            NoSk: $scope.persetujuanModel.noSK.toUpperCase(),
            TanggalSk: $('#tanggalSuratPersetujuan').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            File: $scope.persetujuanModel.file

        })
        .success(function (data) {
            debugger;
            $scope.loadPersetujuan();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Kronologi Tahapan';
            $scope.modelUpload.moduleType = '4'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initPersetujuanModel();
            $('#static3').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditDataPersetujuan = false;
        })
    }
    //=============================================================== Persetujuan ==================================

    //=============================================================== Kronologi Tahapan ==================================
    $scope.tambahKronologiTahapanClick = function () {
        $scope.initKronologiTahapanModel();
        $('form[name="dataKronologiTahapanForm"]')[0].reset();
    }

    $scope.CreateKronologiTahapan = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateKronologiTahapan', {
            ID: '-',
            CompanyID: $scope.idx,
            Tahapan: $scope.kronologiTahapanModel.tahapan,
            NoSk: $scope.kronologiTahapanModel.noSk,
            TanggalSk: $('#tanggalSkKt').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.tanggalSk,
            TanggalMulai: $('#mulaiKt').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.mulai,
            TanggalAkhir: $('#sdKt').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.sampaiDengan,
            Keterangan: $scope.kronologiTahapanModel.keterangan,
            JangkaWaktu: $scope.kronologiTahapanModel.jangkaWaktu,
            File: $scope.kronologiTahapanModel.file
        })
        .success(function (data) {
            $('#static2').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadKronologiTahapan();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Kronologi Tahapan';
            $scope.modelUpload.moduleType = '2'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initKronologiTahapanModel();
        })
    }

    $scope.loadKronologiTahapan = function () {
        $http.get('/pkp2b/memasukandata/LoadKronologiTahapan/' + $scope.idx)
        .success(function (data) {
            $scope.dataKronologiTahapan = data.data;
        })
    }
    $scope.loadKronologiTahapan();

    $scope.deleteKronologiTahapan = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteKronologiTahapan', { id: id })
            .success(function (data) {
                $scope.loadKronologiTahapan();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editKronologiTahapanClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindKronologiTahapan/' + id)
        .success(function (data) {
            debugger;
            $scope.kronologiTahapanModel.id = data.ID,
            $scope.kronologiTahapanModel.tahapan = data.Tahapan,
            $scope.kronologiTahapanModel.noSk = data.NoSk,
            $scope.kronologiTahapanModel.keterangan = data.Keterangan,
            $scope.kronologiTahapanModel.jangkaWaktu = data.JangkaWaktu,
            $scope.kronologiTahapanModel.file = data.File,
            $("#tanggalSkKt").datepicker("update", data.TanggalSk);
            $("#mulaiKt").datepicker("update", data.TanggalMulai);
            $("#sdKt").datepicker("update", data.TanggalAkhir);
            $('#static2').modal('show');
            $scope.isEditKronologiTahapan = true;
        })
    }

    $scope.SaveChangesKronologiTahapan = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceKronologiTahapan/', {
            ID: $scope.kronologiTahapanModel.id,
            CompanyID: $scope.idx,
            Tahapan: $scope.kronologiTahapanModel.tahapan,
            NoSk: $scope.kronologiTahapanModel.noSk,
            TanggalSk: $('#tanggalSkKt').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.tanggalSk,
            TanggalMulai: $('#mulaiKt').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.mulai,
            TanggalAkhir: $('#sdKt').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.sampaiDengan,
            Keterangan: $scope.kronologiTahapanModel.keterangan,
            JangkaWaktu: $scope.kronologiTahapanModel.jangkaWaktu,
            File: $scope.kronologiTahapanModel.file

        })
        .success(function (data) {
            debugger;
            $scope.loadKronologiTahapan();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Kronologi Tahapan';
            $scope.modelUpload.moduleType = '2'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initKronologiTahapanModel();
            $('#static2').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditKronologiTahapan = false;
        })
    }
    //=============================================================== Kronologi Tahapan ==================================

    //=============================================================== Detail Kronologi Tahapan ==================================
    $scope.tambahDetailKronologiTahapanClick = function () {
        $scope.initDetailKronologiTahapanModel();
        $('form[name="dataDetailKronologiTahapanForm"]')[0].reset();
    }

    $scope.CreateDetailKronologiTahapan = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateDetailKronologiTahapan', {
            ID: '-',
            CompanyID: $scope.idx,
            Tahapan: $scope.detailKronologiTahapanModel.tahapan,
            NoSk: $scope.detailKronologiTahapanModel.noSk,
            TanggalSk: $('#tanggalSkKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.tanggalSk,
            TanggalMulai: $('#mulaiKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.mulai,
            TanggalAkhir: $('#sdKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.sampaiDengan,
            Keterangan: $scope.detailKronologiTahapanModel.keterangan,
            TahapanInduk: $scope.detailKronologiTahapanModel.tahapanInduk,
            JangkaWaktu: $scope.detailKronologiTahapanModel.jangkaWaktu,
            File: $scope.detailKronologiTahapanModel.file
        })
        .success(function (data) {
            $('#static8').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadDetailKronologiTahapan();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Detail Kronologi Tahapan';
            $scope.modelUpload.moduleType = '3'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initDetailKronologiTahapanModel();
        })
    }

    $scope.loadDetailKronologiTahapan = function () {
        $http.get('/pkp2b/memasukandata/LoadDetailKronologiTahapan/' + $scope.idx)
        .success(function (data) {
            $scope.dataDetailKronologiTahapan = data.data;
        })
    }
    $scope.loadDetailKronologiTahapan();

    $scope.deleteDetailKronologiTahapan = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteDetailKronologiTahapan', { id: id })
            .success(function (data) {
                $scope.loadDetailKronologiTahapan();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editDetailKronologiTahapanClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindDetailKronologiTahapan/' + id)
        .success(function (data) {
            debugger;
            //ID = data.ID,
            //    CompanyID = data.CompanyID,

            //    Tahapan = data.Tahapan,
            //    NoSk = data.NoSk,
            //    TanggalSk = data.TanggalSk.Value.ToString("dd/MM/yyyy"),
            //    TanggalMulai = data.TanggalMulai.Value.ToString("dd/MM/yyyy"),
            //    TanggalAkhir = data.TanggalAkhir.Value.ToString("dd/MM/yyyy"),
            //    Keterangan = data.Keterangan,
            //    TahapanInduk = data.TahapanInduk,
            //    JangkaWaktu = data.JangkaWaktu,
            //    File = data.File

            $scope.detailKronologiTahapanModel.id = data.ID,
            $scope.detailKronologiTahapanModel.tahapan = data.Tahapan,
            $scope.detailKronologiTahapanModel.noSk = data.NoSk,
            $scope.detailKronologiTahapanModel.keterangan = data.Keterangan,
            $scope.detailKronologiTahapanModel.jangkaWaktu = data.JangkaWaktu,
            $scope.detailKronologiTahapanModel.tahapanInduk = data.TahapanInduk,
            $scope.detailKronologiTahapanModel.file = data.File,
            $("#tanggalSkKtDetail").datepicker("update", data.TanggalSk);
            $("#mulaiKtDetail").datepicker("update", data.TanggalMulai);
            $("#sdKtDetail").datepicker("update", data.TanggalAkhir);

            $('#static8').modal('show');
            $scope.isEditDetailKronologiTahapan = true;
        })
    }

    $scope.SaveChangesDetailKronologiTahapan = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceDetailKronologiTahapan/', {

            ID: $scope.detailKronologiTahapanModel.id,
            CompanyID: $scope.idx,
            Tahapan: $scope.detailKronologiTahapanModel.tahapan,
            NoSk: $scope.detailKronologiTahapanModel.noSk,
            TanggalSk: $('#tanggalSkKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.tanggalSk,
            TanggalMulai: $('#mulaiKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.mulai,
            TanggalAkhir: $('#sdKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.sampaiDengan,
            Keterangan: $scope.detailKronologiTahapanModel.keterangan,
            JangkaWaktu: $scope.detailKronologiTahapanModel.jangkaWaktu,
            TahapanInduk: $scope.detailKronologiTahapanModel.tahapanInduk,
            File: $scope.detailKronologiTahapanModel.file

        })
        .success(function (data) {
            debugger;
            $scope.loadDetailKronologiTahapan();

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Detail Kronologi Tahapan';
            $scope.modelUpload.moduleType = '3'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initDetailKronologiTahapanModel();
            $('#static8').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditDetailKronologiTahapan = false;
        })
    }
    //=============================================================== Detail Kronologi Tahapan ==================================


    //=============================================================== Biaya Eksplorasi ==================================

    $scope.loadBiayaEksplorasi = function () {
        $http.get('/pkp2b/memasukandata/loadBiayaEksplorasi/' + $scope.idx)
        .success(function (data) {
            $scope.dataBiayaEksplorasi = data.data;
            if (data.data[0] == undefined == false) {
                $scope.biayaEksplorasiModel.id = data.data[0].ID;
                $scope.biayaEksplorasiModel.mataUang = data.data[0].MataUang;
                $scope.biayaEksplorasiModel.jumlahUang = data.data[0].JumlahUang;
            }
        })
    }
    $scope.loadBiayaEksplorasi();

    $scope.SaveEditBiayaEksplorasi = function () {
        //biayaEksplorasiForm
        if ($scope.biayaEksplorasiForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        if ($scope.biayaEksplorasiModel.id == '') {
            $http.post('/pkp2b/memasukandata/SaveBiayaEksplorasi', {
                ID: '-',
                MataUang: $scope.biayaEksplorasiModel.mataUang,
                JumlahUang: $scope.biayaEksplorasiModel.jumlahUang,
                CompanyID: $scope.idx,
            })
            .success(function (data) {
                toastr["success"]("Data Berhasil Disimpan..");
                swal("Sukses!", "Data Terupdate", "success");
            })
        }
        else {
            $http.post('/pkp2b/memasukandata/EditBiayaEksplorasi', {
                ID: $scope.biayaEksplorasiModel.id,
                MataUang: $scope.biayaEksplorasiModel.mataUang,
                JumlahUang: $scope.biayaEksplorasiModel.jumlahUang,
                CompanyID: $scope.idx,
            })
            .success(function (data) {
                toastr["success"]("Data Berhasil Disimpan..");
                swal("Sukses!", "Data Terupdate", "success");
            })
        }
    }

    //=============================================================== Biaya Eksplorasi ==================================


    //=============================================================== Genral information ==================================
    $scope.SaveEditGeneralInformation = function () {        
        if ($scope.userForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/EditServiceGeneralInformation', {
            ID: $scope.idx,
            Name: $scope.companyModel.nama.toUpperCase(),
            NPWP: $('#npwp').val(),//$scope.companyModel.npwp,//$('#NpWp').val(),
            Telp: $scope.companyModel.telp,
            Fax: $scope.companyModel.fax,
            Alamat: $scope.companyModel.alamat,
            Email: $scope.companyModel.email,
            Website: $scope.companyModel.website,
            CpNama: $scope.companyModel.namacp == null ? '' : $scope.companyModel.namacp.toUpperCase(),
            CpHp: $scope.companyModel.hpcp,
            Generasi: $scope.dataPkp2bModel.generasi,
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
            if (data == '0')
                alert('fail edit data')
            else             
            {
                toastr["success"]("Data Berhasil Disimpan..");
                swal("Sukses!", "Data Terupdate", "success");
                $("#gridHistory").data("kendoGrid").dataSource.read();
                $("#gridHistory").data("kendoGrid").refresh();
            }
        })
    }

    $scope.loadDetails = function () {
        $http.get('/pkp2b/memasukandata/loaddetails/' + $scope.idx)
        .success(function (data) {
            $scope.companyModel.nama = data.company[0].Name,
            $scope.companyModel.npwp = data.company[0].Npwp,
            $scope.companyModel.telp = data.company[0].Telp,
            $scope.companyModel.fax = data.company[0].Fax,
            $scope.companyModel.alamat = data.company[0].Alamat,            
            $scope.companyModel.email = data.company[0].Email,
            $scope.companyModel.website = data.company[0].Website,
            $scope.companyModel.namacp = data.company[0].CpNama,
            $scope.companyModel.hpcp = data.company[0].CpHp,
            $scope.dataPkp2bModel.generasi = data.datapkp2b.Generasi,
            $scope.dataPkp2bModel.perizinan = data.datapkp2b.Perizinan,
            $scope.dataPkp2bModel.kodeWilayah = data.datapkp2b.KodeWilayah,
            $scope.dataPkp2bModel.noKontrak = data.datapkp2b.NoKontrak,
            $scope.dataPkp2bModel.tanggalKontrak = data.datapkp2b.TanggalKontrak,
            //$('#tanggalKontrak').val(Date(data.datapkp2b.tanggalKontrak)),
            $('#tanggalKontrak').datepicker('update', data.datapkp2b.TanggalKontrak), //data.datapkp2b.TanggalKontrak,
            $scope.dataPkp2bModel.luasWilayahAwal = data.datapkp2b.LuasWilayahAwal,
            $scope.dataPkp2bModel.luasWilayahDipertahankan = data.datapkp2b.LuasWilayahDipertahankan,
            $scope.dataPkp2bModel.provinsi = data.datapkp2b.Provinsi,
            $scope.dataPkp2bModel.kabupaten = data.datapkp2b.Kabupaten,
            $scope.dataPkp2bModel.tahapanAkhir = data.datapkp2b.TahapanAkhir,
            $scope.dataPkp2bModel.tanggalBerakhir = data.datapkp2b.TanggalBerakhir,            
            $('#tanggalBerakhir').datepicker('update', data.datapkp2b.TanggalBerakhir),//data.datapkp2b.TanggalBerakhir,
            $scope.dataPkp2bModel.jangkaWaktu = data.datapkp2b.JangkaWaktu,

            $('#tanggalBerakhir').datepicker('disable');
        })
        .finally(function () { })
    }
    $scope.loadDetails();
    //=============================================================== Genral information ==================================

    //=============================================================== cadangan batubara ==================================
    $scope.tambahCadanganBatubaraClick = function () {
        $scope.initCadanganBatubaraModel();
    }

    $scope.CreateCadanganBatubara = function () {
        debugger;
        if ($scope.dataCadanganBatubaraForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateCadanganBatubara', {
            ID: '-',
            Blok: $scope.cadanganBatubaraModel.blok,
            Terkira: $scope.cadanganBatubaraModel.terkira,
            Terbukti: $scope.cadanganBatubaraModel.terbukti,
            Sr: $scope.cadanganBatubaraModel.sr,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $('#static4').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadCadanganBatubara();
        })
        .finally(function () {
            $scope.initCadanganBatubaraModel();
        })
    }

    $scope.loadCadanganBatubara = function () {
        $http.get('/pkp2b/memasukandata/loadCadanganBatubara/' + $scope.idx)
        .success(function (data) {
            $scope.dataCadanganBatubara = data.data;
        })
    }
    $scope.loadCadanganBatubara();

    $scope.deleteCadanganBatubara = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteCadanganBatubara', { id: id })
            .success(function (data) {
                $scope.loadCadanganBatubara();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editCadanganBatubaraClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindCadanganBatubara/' + id)
        .success(function (data) {
            debugger;
            $scope.cadanganBatubaraModel.id = data.ID,
            $scope.cadanganBatubaraModel.blok = data.Blok,
            $scope.cadanganBatubaraModel.terkira = data.Terkira,
            $scope.cadanganBatubaraModel.terbukti = data.Terbukti,
            $scope.cadanganBatubaraModel.sr = data.Sr

            //$("#tanggalPk").datepicker("update", data.TanggalPengesahanKemenkumhan);
            //$("#tanggalAkta").datepicker("update", data.TanggalAkta);
            $('#static4').modal('show');
            $scope.isEditCadanganBatubara = true;
        })
    }
    $scope.SaveChangesCadanganBatubara = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceCadanganBatubara/', {
            ID: $scope.cadanganBatubaraModel.id,
            CompanyID: $scope.idx,
            Blok: $scope.cadanganBatubaraModel.blok,
            Terkira: $scope.cadanganBatubaraModel.terkira,
            Terbukti: $scope.cadanganBatubaraModel.terbukti,
            Sr: $scope.cadanganBatubaraModel.sr

        })
        .success(function (data) {
            debugger;
            $scope.loadCadanganBatubara();
        })
        .finally(function () {
            $scope.initCadanganBatubaraModel();
            $('#static4').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditCadanganBatubara = false;
        })
    }
    //=============================================================== cadangan batubara ==================================

    //=============================================================== kualitas batubara ==================================
    $scope.tambahKualitasBatubaraClick = function () {
        $scope.initKualiatasBatubaraModel();
    }

    $scope.CreateKualitasBatubara = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateKualitasBatubara', {

            ID: '-',
            Parameter: $scope.kualitasBatubaraModel.parameter,

            BlokArea: $scope.kualitasBatubaraModel.blok,
            SampleCode: $scope.kualitasBatubaraModel.sampleCode,

            TM: $scope.kualitasBatubaraModel.tm,
            IM: $scope.kualitasBatubaraModel.im,
            ASH: $scope.kualitasBatubaraModel.ash,
            VM: $scope.kualitasBatubaraModel.vm,
            FC: $scope.kualitasBatubaraModel.fc,
            TS: $scope.kualitasBatubaraModel.ts,
            CVadb: $scope.kualitasBatubaraModel.cvadb,
            CVdaf: $scope.kualitasBatubaraModel.cvdaf,
            CVar: $scope.kualitasBatubaraModel.cvar,
            
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $('#static5').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadKualitasBatubara();
        })
        .finally(function () {
            $scope.initKualiatasBatubaraModel();
        })
    }

    $scope.loadKualitasBatubara = function () {
        $http.get('/pkp2b/memasukandata/loadKualitasBatubara/' + $scope.idx)
        .success(function (data) {
            $scope.dataKualitasBatubara = data.data;
        })
    }
    $scope.loadKualitasBatubara();

    $scope.deleteKualitasBatubara = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteKualitasBatubara', { id: id })
            .success(function (data) {
                $scope.loadKualitasBatubara();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editKualitasBatubaraClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindKualitasBatubara/' + id)
        .success(function (data) {
            debugger;
            $scope.kualitasBatubaraModel.id = data.ID,
            $scope.kualitasBatubaraModel.parameter = data.Parameter,

            $scope.kualitasBatubaraModel.blok = data.BlokArea,
            $scope.kualitasBatubaraModel.sampleCode = data.SampleCode,

            $scope.kualitasBatubaraModel.tm = data.TM,
            $scope.kualitasBatubaraModel.im = data.IM,
            $scope.kualitasBatubaraModel.ash = data.ASH,

            $scope.kualitasBatubaraModel.vm = data.VM,
            $scope.kualitasBatubaraModel.fc = data.FC,
            $scope.kualitasBatubaraModel.ts = data.TS,
            $scope.kualitasBatubaraModel.cvadb = data.CVadb,
            $scope.kualitasBatubaraModel.cvdaf = data.CVdaf,
            $scope.kualitasBatubaraModel.cvar = data.CVar

            //$("#tanggalPk").datepicker("update", data.TanggalPengesahanKemenkumhan);
            //$("#tanggalAkta").datepicker("update", data.TanggalAkta);
            $('#static5').modal('show');
            $scope.isEditKualitasBatubara = true;
        })
    }
    $scope.SaveChangesKualitasBatubara = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceKualitasBatubara/', {
            ID: $scope.kualitasBatubaraModel.id,
            Parameter: $scope.kualitasBatubaraModel.parameter,


            BlokArea: $scope.kualitasBatubaraModel.blok,
            SampleCode: $scope.kualitasBatubaraModel.sampleCode,

            TM: $scope.kualitasBatubaraModel.tm,
            IM: $scope.kualitasBatubaraModel.im,
            ASH: $scope.kualitasBatubaraModel.ash,
            VM: $scope.kualitasBatubaraModel.vm,
            FC: $scope.kualitasBatubaraModel.fc,
            TS: $scope.kualitasBatubaraModel.ts,
            CVadb: $scope.kualitasBatubaraModel.cvadb,
            CVdaf: $scope.kualitasBatubaraModel.cvdaf,
            CVar: $scope.kualitasBatubaraModel.cvar,
            CompanyID: $scope.idx

        })
        .success(function (data) {
            debugger;
            $scope.loadKualitasBatubara();
        })
        .finally(function () {
            $scope.initKualiatasBatubaraModel();
            $('#static5').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditKualitasBatubara = false;
        })
    }
    //=============================================================== kualitas batubara ==================================

    //=============================================================== Tahap Kegiatan ==================================
    $scope.tambahTahapKegiatanClick = function () {
        $scope.initTahapKegiatanModel();
    }

    $scope.CreateTahapKegiatan = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateTahapKegiatan', {

            ID: '-',
            Tahap: $scope.tahapKegiatanModel.tahapKegiatan,
            LuasDipertahankan: $scope.tahapKegiatanModel.luasDipertahankan,

            CompanyID: $scope.idx
        })
        .success(function (data) {
            $('#static6').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadTahapKegiatan();
        })
        .finally(function () {
            $scope.initTahapKegiatanModel();
        })
    }

    $scope.loadTahapKegiatan = function () {
        $http.get('/pkp2b/memasukandata/loadTahapKegiatan/' + $scope.idx)
        .success(function (data) {
            $scope.dataTahapKegiatan = data.data;
        })
    }
    $scope.loadTahapKegiatan();

    $scope.deleteTahapKegiatan = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteTahapKegiatan', { id: id })
            .success(function (data) {
                $scope.loadTahapKegiatan();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editTahapKegiatanClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindTahapKegiatan/' + id)
        .success(function (data) {
            debugger;
            $scope.tahapKegiatanModel.id = data.ID,
            $scope.tahapKegiatanModel.tahapKegiatan = data.Tahap,
            $scope.tahapKegiatanModel.luasDipertahankan = data.LuasDipertahankan

            //$("#tanggalPk").datepicker("update", data.TanggalPengesahanKemenkumhan);
            //$("#tanggalAkta").datepicker("update", data.TanggalAkta);
            $('#static6').modal('show');
            $scope.isEditTahapKegiatan = true;
        })
    }
    $scope.SaveChangesTahapKegiatan = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceTahapKegiatan/', {
            //ID: '-',
            //Tahap: $scope.tahapKegiatanModel.tahapKegiatan,
            //LuasDipertahankan: $scope.tahapKegiatanModel.luasDipertahankan,
            //CompanyID: $scope.idx

            ID: $scope.tahapKegiatanModel.id,
            CompanyID: $scope.idx,
            Tahap: $scope.tahapKegiatanModel.tahapKegiatan,
            LuasDipertahankan: $scope.tahapKegiatanModel.luasDipertahankan

        })
        .success(function (data) {
            debugger;
            $scope.loadTahapKegiatan();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'SK Perpanjangan';
            //$scope.modelUpload.moduleType = '3'
            //$scope.uploadFile();
        })
        .finally(function () {
            $scope.initTahapKegiatanModel();
            $('#static6').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditTahapKegiatan = false;
        })
    }
    //=============================================================== Tahap Kegiatan ==================================

    //=============================================================== Tahap Kegiatan Blok ==================================
    $scope.tambahTahapKegiatanBlokClick = function (id) {
        $scope.initTahapKegiatanBlokModel();
        $scope.tahapKegiatanBlokModel.id = id;
    }

    $scope.CreateTahapKegiatanBlok = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateTahapKegiatanBlok', {
            ID: '-',
            Tahap: $scope.tahapKegiatanBlokModel.tahapKegiatan,
            LuasDipertahankan: $scope.tahapKegiatanBlokModel.luasDipertahankan,
            TahapkegiatanId: $scope.tahapKegiatanBlokModel.id,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $('#detailStatic6').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadTahapKegiatanBlok();
        })
        .finally(function () {
            $scope.initTahapKegiatanBlokModel();
        })
    }

    $scope.loadTahapKegiatanBlok = function () {
        $http.get('/pkp2b/memasukandata/loadTahapKegiatanBlok/' + $scope.idx)
        .success(function (data) {
            $scope.dataTahapKegiatanBlok = data.data;
        })
    }
    $scope.loadTahapKegiatanBlok();

    $scope.deleteTahapKegiatanBlok = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteTahapKegiatanBlok', { id: id })
            .success(function (data) {
                $scope.loadTahapKegiatanBlok();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editTahapKegiatanBlokClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindTahapKegiatanBlok/' + id)
        .success(function (data) {
            debugger;
            $scope.tahapKegiatanBlokModel.id = data.ID,
            $scope.tahapKegiatanBlokModel.tahapKegiatan = data.Tahap,
            $scope.tahapKegiatanBlokModel.tahapkegiatanId = data.TahapKegiatanId,
            $scope.tahapKegiatanBlokModel.luasDipertahankan = data.LuasDipertahankan

            //$("#tanggalPk").datepicker("update", data.TanggalPengesahanKemenkumhan);
            //$("#tanggalAkta").datepicker("update", data.TanggalAkta);
            $('#detailStatic6').modal('show');
            $scope.isEditTahapKegiatanBlok = true;
        })
    }
    $scope.SaveChangesTahapKegiatanBlok = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceTahapKegiatanBlok/', {

            ID: $scope.tahapKegiatanBlokModel.id,
            CompanyID: $scope.idx,
            Tahap: $scope.tahapKegiatanBlokModel.tahapKegiatan,
            TahapKegiatanId: $scope.tahapKegiatanBlokModel.tahapkegiatanId,
            LuasDipertahankan: $scope.tahapKegiatanBlokModel.luasDipertahankan

        })
        .success(function (data) {
            debugger;
            $scope.loadTahapKegiatanBlok();
        })
        .finally(function () {
            $scope.initTahapKegiatanBlokModel();
            $('#detailStatic6').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditTahapKegiatanBlok = false;
        })
    }
    //=============================================================== Tahap Kegiatan Blok ==================================


    //=============================================================== Sumber Daya Batubara ==================================
    $scope.tambahSumberDayaBatubaraClick = function () {
        $scope.initSumberDayaBatubaraModel();
    }

    $scope.CreateSumberDayaBatubara = function () {
        debugger;
        //if ($scope.dataAktaForm.$invalid) { return; }
        toastr["info"]("Menyimpan Data..");
        $http.post('/pkp2b/memasukandata/CreateSumberDayaBatubara', {
            /*
             public string Blok { get; set; }
        public int Tereka{ get; set; }
        public int Terunjuk { get; set; }
        public int Terukur { get; set; }        

           id: '',
            blok: '',
            tereka: '',
            terunjuk: '',
            terukur: ''
            */

            ID: '-',
            Blok: $scope.SumberDayaBatubaraModel.blok,
            Tereka: $scope.SumberDayaBatubaraModel.tereka,
            Terunjuk: $scope.SumberDayaBatubaraModel.terunjuk,
            Terukur: $scope.SumberDayaBatubaraModel.terukur,

            CompanyID: $scope.idx
        })
        .success(function (data) {
            $('#static7').modal('hide')
            toastr["success"]("Data Berhasil Disimpan..");
            $scope.loadSumberDayaBatubara();
        })
        .finally(function () {
            $scope.initSumberDayaBatubaraModel();
        })
    }

    $scope.loadSumberDayaBatubara = function () {
        $http.get('/pkp2b/memasukandata/loadSumberDayaBatubara/' + $scope.idx)
        .success(function (data) {
            $scope.dataSumberDayaBatubara = data.data;
        })
    }
    $scope.loadSumberDayaBatubara();

    $scope.deleteSumberDayaBatubara = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/pkp2b/MemasukanData/DeleteSumberDayaBatubara', { id: id })
            .success(function (data) {
                $scope.loadSumberDayaBatubara();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.editSumberDayaBatubaraClick = function (id) {
        $http.get('/pkp2b/memasukandata/FindSumberDayaBatubara/' + id)
        .success(function (data) {
            debugger;
            $scope.SumberDayaBatubaraModel.id = data.ID,
            $scope.SumberDayaBatubaraModel.blok = data.Blok,
            $scope.SumberDayaBatubaraModel.tereka = data.Tereka,
            $scope.SumberDayaBatubaraModel.terunjuk = data.Terunjuk,
            $scope.SumberDayaBatubaraModel.terukur = data.Terukur

            //$("#tanggalPk").datepicker("update", data.TanggalPengesahanKemenkumhan);
            //$("#tanggalAkta").datepicker("update", data.TanggalAkta);
            $('#static7').modal('show');
            $scope.isEditSumberDayaBatubara = true;
        })
    }
    $scope.SaveChangesSumberDayaBatubara = function (id) {
        debugger;
        $http.post('/pkp2b/memasukandata/EditServiceSumberDayaBatubara/', {
            //ID: '-',
            //Blok: $scope.SumberDayaBatubaraModel.blok,
            //Tereka: $scope.SumberDayaBatubaraModel.tereka,
            //Terunjuk: $scope.SumberDayaBatubaraModel.terunjuk,
            //Terukur: $scope.SumberDayaBatubaraModel.terukur,

            ID: $scope.SumberDayaBatubaraModel.id,
            CompanyID: $scope.idx,
            Blok: $scope.SumberDayaBatubaraModel.blok,
            Tereka: $scope.SumberDayaBatubaraModel.tereka,
            Terunjuk: $scope.SumberDayaBatubaraModel.terunjuk,
            Terukur: $scope.SumberDayaBatubaraModel.terukur

        })
        .success(function (data) {
            debugger;
            $scope.loadSumberDayaBatubara();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'SK Perpanjangan';
            //$scope.modelUpload.moduleType = '3'
            //$scope.uploadFile();
        })
        .finally(function () {
            $scope.initSumberDayaBatubaraModel();
            $('#static7').modal('hide');
            toastr["success"]("Data Berhasil Diubah..");
            $scope.isEditSumberDayaBatubara = false;
        })
    }
    //=============================================================== Sumber Daya Batubara ==================================


    //=============================================================== Share Holder ==================================

    $scope.loadShareholders = function () {
        $http.get('/organization/shareholders/loadshareholders/' + $scope.idx)
        .success(function (data) {            
            $scope.shareHolders = data;
        })
        .finally(function () { })
    }
    $scope.loadShareholders();


    $scope.saveShareholder = function () {
        debugger;
        if ($scope.shareholderModel.isActive == '1')
        { $scope.shareholderModel.isActive = true }
        else { $scope.shareholderModel.isActive = false }
        if ($scope.shareholderModel.nama.length == 0) return false;
        if ($scope.isEditShareholder == true) {
            $http.post('/organization/shareholders/editservice', {
                ID: $scope.shareholderModel.id,
                Name: $scope.shareholderModel.nama.toUpperCase(),
                CompanyID: $scope.idx,
                TotalStock: $scope.shareholderModel.jumlahSaham,
                StatusWnBh: $scope.shareholderModel.statuswnbh,
                Currency: $scope.shareholderModel.nilaiSaham,
                Country: $scope.shareholderModel.negara,
                ProsentaseSaham: $scope.shareholderModel.prosentaseSaham
            })
              .success(function (data) {
                  $scope.loadShareholders();
                  $scope.initShareholderModel();
              })
              .error(function () {
                  $scope.initShareholderModel();
              })
        }
        else {
            $http.post('/pkp2b/memasukandata/CreateServiceShareHolder', {
                ID: '-',
                Name: $scope.shareholderModel.nama.toUpperCase(),
                CompanyID: $scope.idx,
                TotalStock: $scope.shareholderModel.jumlahSaham,
                StatusWnBh: $scope.shareholderModel.statuswnbh,
                Currency: $scope.shareholderModel.nilaiSaham,
                Country: $scope.shareholderModel.negara,
                ProsentaseSaham: $scope.shareholderModel.prosentaseSaham
            })
              .success(function (data) {
                  $scope.loadShareholders();
                  $scope.initShareholderModel();
              })
              .error(function () {
                  $scope.initShareholderModel();
              })
        }
        $scope.isEditShareholder == false;
    }
    $scope.editShareHolderClick = function (id) {
        $scope.isEditShareholder = true;
        $http.get('/organization/shareholders/findbyid/' + id)
        .success(function (data) {
            $scope.shareholderModel.id = data.ID;
            $scope.shareholderModel.nama = data.Name.toUpperCase();
            $scope.shareholderModel.jumlahSaham = data.TotalStock;
            $scope.shareholderModel.isActive = data.Status;
            $scope.shareholderModel.prosentaseSaham = data.ProsentaseSaham;
            $scope.shareholderModel.nilaiSaham = data.Currency;
            $scope.shareholderModel.negara = data.Country;
            $scope.shareholderModel.statuswnbh = data.StatusWnBh;
            $timeout(function () { if (data.Status == true) {
                $('#shareholderStatus').val(1)
            } else {
                $('#shareholderStatus').val(0)
            } }, 1000);           
        });
    }

    $scope.deleteShareholder = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/organization/shareholders/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadShareholders();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    //=============================================================== Share Holder ==================================


    //=============================================================== Direksi ==================================
    $scope.saveDirector = function () {
        if ($scope.DirectorModel.isActive == '1')
        { $scope.DirectorModel.isActive = true }
        else { $scope.DirectorModel.isActive = false }
        if ($scope.DirectorModel.nama.length == 0) return false;
        var status = 0;
        if ($scope.isEditDirector == true) {
            $http.post('/organization/Directors/editservice', {
                ID: $scope.DirectorModel.ID,
                Name: $scope.DirectorModel.nama.toUpperCase(),
                CompanyID: $scope.idx,
                Status: $scope.DirectorModel.status,
                Country: $scope.DirectorModel.negara,
                AdditionalInformation: $scope.DirectorModel.keterangan
            })
              .success(function (data) {
                  $scope.loadDirectors();
                  $scope.initDirectorModel();
              })
              .error(function () {
                  $scope.initDirectorModel();
              })
        }
        else {
            $http.post('/organization/Directors/createservice', {
                ID: '-',
                Name: $scope.DirectorModel.nama.toUpperCase(),
                CompanyID: $scope.idx,                
                Status: $scope.DirectorModel.status,
                Country: $scope.DirectorModel.negara,
                AdditionalInformation: $scope.DirectorModel.keterangan
            })
              .success(function (data) {
                  $scope.loadDirectors();
                  $scope.initDirectorModel();
              })
              .error(function () {
                  $scope.initDirectorModel();
              })
        }
        $scope.isEditDirector == false;
    }

    $scope.loadDirectors = function () {
        $http.get('/organization/Directors/loadDirectors/' + $scope.idx)
        .success(function (data) {

            $scope.Directors = data;
        })
        .finally(function () { })
    }
    $scope.loadDirectors();

    $scope.deleteDirector = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/organization/Directors/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadDirectors();
                swal("Sukses!", "Data terhapus", "success");
                $scope.isEditDirector = false;
            })
        });
    }

    $scope.editDirectorClick = function (id) {
        $scope.isEditDirector = true;
        $http.get('/organization/Directors/findbyid/' + id)
        .success(function (data) {
            $scope.DirectorModel.ID = data.ID;
            $scope.DirectorModel.nama = data.Name;
            $scope.DirectorModel.status = data.Status;
            $scope.DirectorModel.CompanyID = $scope.idx,
            $scope.DirectorModel.negara = data.Country;
            $scope.DirectorModel.keterangan = data.AdditionalInformation;            
        });
    }
    //=============================================================== Direksi ==================================

    //=============================================================== Komisaris ==================================

    $scope.loadCommisioners = function () {
        $http.get('/organization/Commissioners/loadCommisioners/' + $scope.idx)
        .success(function (data) {
            $scope.Commisioners = data;
        })
        .finally(function () { })
    }

    $scope.loadCommisioners();

    $scope.saveCommisioner = function () {
        if ($scope.CommisionerModel.isActive == '1')
        { $scope.CommisionerModel.isActive = true }
        else { $scope.CommisionerModel.isActive = false }
        if ($scope.CommisionerModel.nama.length == 0) return false;
        var status = 0;
        if ($scope.isEditCommisioner == true) {
            $http.post('/organization/Commissioners/editservice', {
                ID: $scope.CommisionerModel.ID,
                Name: $scope.CommisionerModel.nama.toUpperCase(),
                CompanyID: $scope.idx,
                Status: $scope.CommisionerModel.status,
                Country: $scope.CommisionerModel.negara,
                AdditionalInformation: $scope.CommisionerModel.keterangan
            })
              .success(function (data) {
                  $scope.loadCommisioners();
                  $scope.initCommisionerModel();
              })
              .error(function () {
                  $scope.initCommisionerModel();
              })
        }
        else {
            $http.post('/organization/Commissioners/createservice', {
                ID: '-',
                Name: $scope.CommisionerModel.nama.toUpperCase(),
                CompanyID: $scope.idx,
                Status: $scope.CommisionerModel.status,
                Country: $scope.CommisionerModel.negara,
                AdditionalInformation: $scope.CommisionerModel.keterangan
            })
              .success(function (data) {
                  $scope.loadCommisioners();
                  $scope.initCommisionerModel();
              })
              .error(function () {
                  $scope.initCommisionerModel();
              })
        }
        $scope.isEditCommisioner == false;

    }

    $scope.deleteCommisioner = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/organization/Commissioners/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadCommisioners();
                swal("Sukses!", "Data terhapus", "success");
                $scope.isEditCommisioner = false;
            })

        });
    }
    $scope.editCommisionerClick = function (id) {
        $scope.isEditCommisioner = true;
        $http.get('/organization/Commissioners/findbyid/' + id)
        .success(function (data) {
            $scope.CommisionerModel.ID = data.ID;
            $scope.CommisionerModel.nama = data.Name;
            $scope.CommisionerModel.status = data.Status;
            $scope.CommisionerModel.negara = data.Country;
            $scope.CommisionerModel.keterangan = data.AdditionalInformation;
            $scope.CommisionerModel.CompanyID = $scope.idx,
            $timeout(function () {
                $('#CommisionerStatus').val(data.Status)

            }, 1000);

        });

    }

    //=============================================================== Komisaris ==================================

})

