


var app = angular.module('iupop', []);
app.filter("mydate", function () {
    return function (x) {
        if(x!= null)
            return new Date(parseInt(x.substr(6)));

    };
});

app.controller('iupop-controller', function ($scope, $http, $interval, $timeout) {
    
    
    $("#masaberlaku").on('change', function () {
        if ($('#tglskawal').val() == '') {
            $('#tglberakhirskawal').val('');
            document.getElementById("tglskawal").focus();
            $('#masaberlaku').val('');
            alert('Tolong Isi Tanggal SK');
        }
        else {
            var length = $('#masaberlaku').val();
            var dateMin = $('#tglskawal').datepicker("getDate");
            var rMax = new Date(dateMin.getFullYear() + parseInt(length), dateMin.getMonth(), dateMin.getDate() - 1);
            $('#tglberakhirskawal').datepicker('update', rMax);
        }
    });

    $("#masaberlaku2").on('change', function () {
        if ($('#tglskawal2').val() == '') {
            $('#tglberakhirskawal2').val('');
            document.getElementById("tglskawal2").focus();
            $('#masaberlaku2').val('');
            alert('Tolong Isi Tanggal SK');
        }
        else {
            var length = $('#masaberlaku2').val();
            var dateMin = $('#tglskawal2').datepicker("getDate");
            var rMax = new Date(dateMin.getFullYear() + parseInt(length), dateMin.getMonth(), dateMin.getDate() - 1);
            $('#tglberakhirskawal2').datepicker('update', rMax);
        }
    });

    $scope.isGeneralInformationEdited = false;
    $scope.modelUpload = {
        id: '',
        title: '',
        moduleType: ''
    };

    $scope.idx = $('#idx').val();
    $('.date-picker').datepicker({
        autoclose : true,
        format : "dd/mm/yyyy"
    });

    function onSelect(e) {
        console.log("Select :: " + getFileInfo(e));
    }

    function onUpload(e) {
        console.log("Upload :: " + getFileInfo(e));
        e.data = {
            id: $scope.modelUpload.id,
            moduleType : $scope.modelUpload.moduleType
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
            $scope.loadSkAwal();
        }
        if ($scope.modelUpload.moduleType == "2")
        {
            $scope.loadSkAwalIupOp();
        }
        else if ($scope.modelUpload.moduleType == "3")
        {
            $scope.loadSkPerpanjangan2();
        }
        else if ($scope.modelUpload.moduleType == "4")
        {
            $scope.loadSkPerpanjanganIupOp();
        }
        else if ($scope.modelUpload.moduleType == "5")
        {
            $scope.loadSkPenyesuaian();
        }
        else if ($scope.modelUpload.moduleType == "6") {
            $scope.loadSkPenghentianSementara();
        }
        else if ($scope.modelUpload.moduleType == "7") {
            $scope.loadSkPencabutanIjin();
        }
        else if ($scope.modelUpload.moduleType == "8") {
            $scope.loadSkPengaktifan();
        }
        else if ($scope.modelUpload.moduleType == "9") {
            $scope.loadSkBkpm();
        }
        else if ($scope.modelUpload.moduleType == "10") {
            $scope.loadSkBkpmIupOp();
        }
        else if ($scope.modelUpload.moduleType == "11") {
            $scope.loadETRecommendation();
        }
        else if ($scope.modelUpload.moduleType == "12") {
            $scope.loadAdditionalCooperation();
        }
        else if ($scope.modelUpload.moduleType == "13") {
            $scope.loadCNCCertificate();
        }
        else if ($scope.modelUpload.moduleType == "14") {
            $scope.loadCompanyFile();
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

    //MODEL

    $scope.initCompanyModel = function () {
        $scope.companyModel = {
            id:'',
            name: '',
            nourutberkas: '',
            npwp: '',
            statusizin: '',
            tahapiup: '',
            iuptype: '',
            address: '',
            telp: '',
            mobile: '',
            email: '',
            website: '',
            remark: '',
            fax: '',
            namacp:''
        }
    }   
    $scope.initCompanyModel();

    $scope.initShareholderModel = function () {
        $scope.shareholderModel = {
            id:'',
            name: '',
            jumlah: '',
            currency: '',
            nilai: '',
            statuswnbh: '',
            isActive :''
        }
    }
    $scope.initShareholderModel();  


    $scope.initCommisionerModel = function () {
        $scope.CommisionerModel = {
            ID: '',
            Name: '',
            NPWP: '',
            Status: '',
            CompanyID: '',
         }
    }
    $scope.initCommisionerModel();

    $scope.initDirectorModel = function () {
        $scope.DirectorModel = {
            ID: '',
            Name: '',
            Status: '',
            CompanyID: '',
            NPWP: '',

        }
    }
    $scope.initDirectorModel();

    $scope.initSkAwalModel = function () {
        $scope.skAwalModel = {
            id:'',
            nosurat: '',
            tanggalpermohonan: '',
            nosk: '',
            tglsk: '',
            tglberakhir: '',
            jangkawaktu: '',
            fileSk: '',
            sertifikatCnc: ''
        }
    }
    $scope.initSkAwalModel();

    $scope.initSkAwalIupOpModel = function () {
        $scope.skAwalIupModel = {
            id:'',
            skNumber: '',
            skDate: '',
            skFile: '',
            companySource: '',
            companySourceAddress: '',
            city: '',
            province: '',
            salesDestination: '',
            remark: '',
            tonnage: '',
            firstSkID: '',
            masaberlaku: '',
            extendedSkID: '',
        }
    }
    $scope.initSkAwalIupOpModel();

    $scope.initSalesDestinationModel = function () {
        $scope.salesDestinationModel = {
            ID: '',
            Destination: '',
            Remark: '',
            FirstSkID: '',
            ExtendedSkID: ''
        }  
    }
    $scope.initSalesDestinationModel();

    $scope.initSkPerpanjanganModel = function () {
        $scope.skPerpanjanganModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkEndDate: '',
            SkDuration: '',
            SertifikatCNC: '',
            SkFile:''
        }
    }
    $scope.initSkPerpanjanganModel();

    $scope.initSkPengakhiranModel = function () {
        $scope.skPengakhiranModel = {
            LetterNumber: '',
            LetterDate: '',
            SkNumber: '',
            SkDate: '',
            SkFile:''
        }
    }
    $scope.initSkPengakhiranModel();


    $scope.initSkPerpanjanganIupOpModel = function () {
        $scope.skPerpanjanganIupModel = {
            
            skNumber: '',
            skDate: '',
            skFile: '',
            companySource: '',
            companySourceAddress: '',
            city: '',
            province: '',
            salesDestination: '',
            remark: '',
            tonnage: '',
            firstSkID: '',
            extendedSkID: '',
        }
    }
    $scope.initSkPerpanjanganIupOpModel();

    $scope.initSalesDestinationSkPerpanjanganModel = function () {
        $scope.salesDestinationSkPerpanjanganModel = {
            ID: '',
            Destination: '',
            Remark: '',
            FirstSkID: '',
            ExtendedSkID: ''
        }
    }
    $scope.initSalesDestinationSkPerpanjanganModel();

    $scope.initSkPenyesuaianModel = function () {
        $scope.skModelPenyesuaian = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initSkPenyesuaianModel();

    $scope.initPerubahanKerjasamaModel = function () {
        $scope.perubahanKerjasamaModel = {
            ID:'',
            LetterNumber: '',
            LetterDate: '',
            SkNumber: '',
            SkDate: '',
            CompanyId: '',
            skFile:''
        }
    }
    $scope.initPerubahanKerjasamaModel();

    $scope.initSumberPerubahanKerjasamaModel = function () {
        $scope.sumberPerubahanKerjasamaModel = {
            ID: '',
            CompanyName: '',
            Volume: '',
            SourceChangesID: '',
            CompanyID: ''
        }
    }
    $scope.initSumberPerubahanKerjasamaModel();

    $scope.initSumberSkPerpanjanganModel = function () {
        $scope.sumberSkPerpanjanganModel = {
            ID: '',
            CompanyName: '',
            Volume: '',
            ExtendedSkID: '',
            CompanyID: ''
        }
    }
    $scope.initSumberSkPerpanjanganModel();

    $scope.initSkPenghentianSementaraModel = function () {
        $scope.skPenghentianSementaraModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initSkPenghentianSementaraModel();


    $scope.initSkPencabutanIjinModel = function () {
        $scope.skPencabutanIjinModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initSkPencabutanIjinModel();

    $scope.initSkPencabutanIjinModel = function () {
        $scope.skPencabutanIjinModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initSkPencabutanIjinModel();


    $scope.initSkPengaktifanModel = function () {
        $scope.skPengaktifanModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initSkPengaktifanModel();

    $scope.initSkBkpmModel = function () {
        $scope.skBkpmModel = {
            ID:'',
            SkNumber: '',
            LetterNumber: '',
            LetterDate: '',
            BKPMAcceptanceDate: '',
            LetterType: '',
            EvaluatorAcceptanceDate: '',
            AdditionalInformation: '',
            AdditionalInformation: '',
            SKFile:''
        }
    }
    $scope.initSkBkpmModel();

    $scope.initSkBkpmIupOpModel = function () {
        $scope.skBkpmIupModel = {
            skNumber: '',
            skDate: '',
            skFile: '',
            companySource: '',
            companySourceAddress: '',
            city: '',
            province: '',
            salesDestination: '',
            remark: '',
            tonnage: '',
            firstSkID: '',
            extendedSkID: '',
        }
    }
    $scope.initSkBkpmIupOpModel();

    $scope.initETRecommendationModel = function () {
        $scope.ETRecommendationModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initETRecommendationModel();


    $scope.initCNCCertificateModel = function () {
        $scope.CNCCertificateModel = {
            LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkAdditionalInfo: '',
            SkDuration: '',
        }
    }
    $scope.initCNCCertificateModel();

    $scope.initCompanyFileModel = function () {
        $scope.CompanyFileModel = {
            ID: '',
            FileName: '',
            NamaSurat: '',
            NomorSurat: '',
            TanggalSurat: '',
            Pengirim: '',
            Tujuan: '',
            Perihal: '',
            Description: '',
            Module: '',
            CompanyID:''
        }
    }
    $scope.initCompanyFileModel();


    $scope.initRkabModel = function () {
        $scope.RkabModel = {
            ID: '',
            CompanyName: '',
            RkabYear: '',
            Status: '',
            CompanyID: ''
        }
    }
    $scope.initRkabModel();

    //SUMBER SK AWAL
    $scope.initFirstSkSourceModel = function () {
        $scope.FirstSkSourceModel = {
            ID: '',
            CompanyName: '',
            Volume: '',
            FirstSkID:''
        }
    }
    $scope.initFirstSkSourceModel();
    //END SUMBER SK AWAL
    //SUMBER SK PERPANJANGAN
    $scope.initExtendedSkSourceModel = function () {
        $scope.ExtendedSkSourceModel = {
            ID: '',
            CompanyName: '',
            Volume: '',
            ExtendedSkID: ''
        }
    }
    $scope.initExtendedSkSourceModel();
    //END SUMBER SK PERPANJANGAN

    $scope.initQuarterModel = function () {
        $scope.QuarterModel = {
            ID: '',
            CompanyName: '',
            Year: '',
            Period: '',
            Status: '',
            CompanyID: ''
        }
    }
    $scope.initRkabModel();



    $scope.initAnnualModel = function () {
        $scope.AnnualModel = {
            ID: '',
            CompanyName: '',
            AnnualYear: '',
            Status: '',
            CompanyID: ''
        }
    }
    $scope.initAnnualModel();


    $scope.initReportModel = function () {
        $scope.ReportModel = {
            ID: '',
            Year: '',
            Rkab: '',
            Q1: '',
            Q2: '',
            Q3: '',
            Q4: '',
            Annual:''
        }
    }
    $scope.initReportModel();

    //END OF MODEL

    //COMPANY DETAILS
    $scope.loadDetails = function () {
        /*
        
                              ID = a.ID,
                              Name = a.Name,
                              NoUrutBerkas = a.NoUrutBerkas,
                              NoTelp = b.TelNumber,
                              NoHand = b.MobileNumber,
                              Email = b.Email,
                              NPWP = a.NPWP,
                              StatusIzin = a.StatusIzin,
                              Address = b.Address,
                              Keterangan = b.AdditionalInfo
        */

        $http.get('/angkutjual/iupopangkutjual/loaddetails/' + $scope.idx)
        .success(function (data) {
            
            $scope.companyModel.id = data.company[0].ID;
            $scope.companyModel.name = data.company[0].Name;
            $scope.companyModel.nourutberkas = data.company[0].NoUrutBerkas;
            $scope.companyModel.npwp = data.company[0].NPWP;
            $scope.companyModel.statusizin = data.company[0].StatusIzin == true ? '1' : '0';
            $scope.companyModel.tahapiup = data.company[0].TahapIup;
            $scope.companyModel.iuptypeid = data.company[0].IupTypeID;
            $scope.companyModel.address = data.company[0].Address;
            $scope.companyModel.telp = data.company[0].NoTelp;
            $scope.companyModel.mobile = data.company[0].NoHand == '' ? '+62' : data.company[0].NoHand;
            $scope.companyModel.email = data.company[0].Email;
            $scope.companyModel.remark = data.company[0].Keterangan;
            $scope.companyModel.website = data.company[0].Website;
            $scope.companyModel.fax = data.company[0].Fax;
            $scope.companyModel.namacp = data.company[0].CPName;
                        
            //$('#stsizin').val($scope.companyModel.statusizin);
            //$scope.companyModel.id = data.company.ID;
            //$scope.companyModel.name = data.company.Name;
            //$scope.companyModel.nourutberkas = data.company.NoUrutBerkas;
            //$scope.companyModel.npwp = data.company.NPWP;
            //$scope.companyModel.statusizin = data.company.StatusIzin;
            //$scope.companyModel.tahapiup = data.company.TahapIup;
            //$scope.companyModel.iuptypeid = data.company.IupTypeID;
            //$scope.companyModel.address = data.company.Address;

            //$scope.companyModel.address = data.companyaddress.address;
            //$scope.companyModel.telp = data.companyaddress.telp;
            //$scope.companyModel.mobile = data.companyaddress.mobile;
            //$scope.companyModel.email = data.companyaddress.email;
            //$scope.companyModel.website = data.companyaddress.website;
            //$scope.companyModel.remark = data.companyaddress.remark;
            //$scope.shareHolders = data.shareHolders;
        })
            .finally(function () { })
    }
    $scope.loadDetails();
    $scope.saveCompanyGeneralInfo = function () {

        /*
                public string ID { get; set; }
        public string Name { get; set; }
        public string NPWP { get; set; }
        public string StatusIzin { get; set; }
        public string TahapIup { get; set; }
        public string NoUrutBerkas { get; set; }
        public string IupTypeID { get; set; }
        public string Address { get; set; }
        public string TelNumber { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Website { get; set; }
        public string AdditionalInfo { get; set; }

         id:'',
            name: '',
            nourutberkas: '',
            npwp: '',
            statusizin: '',
            tahapiup: '',
            iuptype: '',
            address: '',
            telp: '',
            mobile: '',
            email: '',
            website: '',
            remark:''
        */
        //debugger;
        $http.post('/organization/companies/SaveCompanyGeneralInfo', {
            ID: $scope.idx,
            Name: $scope.companyModel.name,
            NPWP: $('#NpWp').val(),//$scope.companyModel.npwp,
            StatusIzin: $scope.companyModel.statusizin,
            TahapIup: $scope.companyModel.tahapiup,
            NoUrutBerkas: $scope.companyModel.nourutberkas,
            IupTypeID: $scope.companyModel.iuptype,
            Address: $scope.companyModel.address,
            TelNumber: $scope.companyModel.telp,
            email: $scope.companyModel.email,
            MobileNumber: $scope.companyModel.mobile,
            Website: $scope.companyModel.website,
            AdditionalInfo: $scope.companyModel.remark,
            Fax: $scope.companyModel.fax,
            CPName: $scope.companyModel.namacp

        })
        .success(function (data) {
            swal("Sukses!", "Data tersimpan", "success");
        })
    }
    //END OF COMPANY DETAILS

    //SHREHOLDER
    $scope.isEditShareholder = false;
    $scope.editShareHolderClick = function (id) {
        $scope.isEditShareholder = true;
        $http.get('/organization/shareholders/findbyid/' + id)
        .success(function (data) {
            $scope.shareholderModel.id = data.ID;
            $scope.shareholderModel.name = data.Name.toUpperCase();
            $scope.shareholderModel.jumlah = data.TotalStock;
            $scope.shareholderModel.isActive = data.Status;
            $scope.shareholderModel.currency = data.Currency;
            $scope.shareholderModel.statuswnbh = data.StatusWnBh;
            $timeout(function () { if (data.Status == true) {
                $('#shareholderStatus').val(1)
            } else {
                $('#shareholderStatus').val(0)
            } }, 1000);           
        });
    }

    $scope.loadShareholders = function () {
        $http.get('/organization/shareholders/loadshareholders/' + $scope.idx)
        .success(function (data) {
            //debugger;
            $scope.shareHolders = data;            
        })
        .finally(function () { })
    }
    $scope.loadShareholders();

    $scope.saveShareholder = function () {        
        if ($scope.shareholderModel.isActive == '1')
        { $scope.shareholderModel.isActive = true }
        else { $scope.shareholderModel.isActive = false }

        if ($scope.shareholderModel.name.length == 0) return false;

        if ($scope.isEditShareholder == true) {
            $http.post('/organization/shareholders/editservice', {
                ID: $scope.shareholderModel.id,
                Name: $scope.shareholderModel.name.toUpperCase(),
                CompanyID: $scope.idx,
                TotalStock: $scope.shareholderModel.jumlah,
                StatusWnBh: $scope.shareholderModel.statuswnbh,
                Currency: $scope.shareholderModel.currency
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
            $http.post('/organization/shareholders/createservice', {
                ID: '-',
                Name: $scope.shareholderModel.name.toUpperCase(),
                CompanyID: $scope.idx,
                TotalStock: $scope.shareholderModel.jumlah,
                StatusWnBh: $scope.shareholderModel.statuswnbh,
                Currency: $scope.shareholderModel.currency
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
    //end of SHARE HOLDER

    //COMMISIONER
    $scope.isEditCommisioner = false;

    $scope.editCommisionerClick = function (id) {
        $scope.isEditCommisioner = true;
        $http.get('/organization/Commissioners/findbyid/' + id)
        .success(function (data) {
            
            $scope.CommisionerModel.ID = data.ID;
            $scope.CommisionerModel.Name = data.Name;            
            $scope.CommisionerModel.NPWP = data.NPWP;
            
            $scope.CommisionerModel.Status = data.Status;
            $scope.CommisionerModel.CompanyID = $scope.idx,
            $timeout(function () {
                $('#CommisionerStatus').val(data.Status)

            }, 1000);

        });

    }

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

        if ($scope.CommisionerModel.Name.length == 0) return false;
        var status = 0;
        if ($scope.isEditCommisioner == true) {
            
            $http.post('/organization/Commissioners/editservice', {
                ID: $scope.CommisionerModel.ID,
                Name: $scope.CommisionerModel.Name.toUpperCase(),
                CompanyID: $scope.idx,
                NPWP: $('#NPWPCommissioner').val(),//$scope.CommisionerModel.NPWP,
                Status: $('#CommisionerStatus').val(),
            })
              .success(function (data) {
                  $scope.loadCommisioners();
                  $scope.initCommisionerModel();
                  $('#NPWPCommissioner').val('')
              })
              .error(function () {
                  $scope.initCommisionerModel();
              })
        }
        else {
            $http.post('/organization/Commissioners/createservice', {
                ID: '-',
                Name: $scope.CommisionerModel.Name.toUpperCase(),
                CompanyID: $scope.idx,
                NPWP: $('#NPWP').val(),
                Status: $('#CommisionerStatus').val(),
            })
              .success(function (data) {
                  $scope.loadCommisioners();
                  $scope.initCommisionerModel();
                  $('#NPWPCommissioner').val('')
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
    //end of COMMISIONER

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

        if ($scope.CommisionerModel.Name.length == 0) return false;
        var status = 0;


        if ($scope.isEditCommisioner == true) {
            $http.post('/organization/Commissioners/editservice', {
                ID: $scope.CommisionerModel.ID,
                Name: $scope.CommisionerModel.Name.toUpperCase(),
                CompanyID: $scope.idx,
                NPWP: $('#NPWPCommissioner').val(),//$scope.CommisionerModel.NPWP,
                Status: $('#CommisionerStatus').val(),
            })
              .success(function (data) {
                  $scope.loadCommisioners();
                  $scope.initCommisionerModel();
                  $('#NPWPCommissioner').val('')
              })
              .error(function () {
                  $scope.initCommisionerModel();
              })
        }
        else {

            $http.post('/organization/Commissioners/createservice', {
                ID: '-',
                Name: $scope.CommisionerModel.Name.toUpperCase(),
                CompanyID: $scope.idx,
                NPWP: $('#NPWPCommissioner').val(),//$scope.CommisionerModel.NPWP,
                Status: $('#CommisionerStatus').val(),
            })
              .success(function (data) {
                  $scope.loadCommisioners();
                  $scope.initCommisionerModel();
                  $('#NPWPCommissioner').val('')
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

    //end of COMMISIONER

    //DIRECTOR
    $scope.isEditDirector = false;

    $scope.editDirectorClick = function (id) {
        $scope.isEditDirector = true;
        $http.get('/organization/Directors/findbyid/' + id)
        .success(function (data) {
            $scope.DirectorModel.ID = data.ID;
            $scope.DirectorModel.Name = data.Name;
            $scope.DirectorModel.NPWP = data.NPWP;
            $scope.DirectorModel.Status = data.Status;
            $scope.DirectorModel.CompanyID = $scope.idx,
            $timeout(function () {
                $('#DirectorStatus').val(data.Status)
            }, 1000);
        });

    }

    $scope.loadDirectors = function () {
        $http.get('/organization/Directors/loadDirectors/' + $scope.idx)
        .success(function (data) {
            
            $scope.Directors = data;
        })
        .finally(function () { })
    }
    $scope.loadDirectors();

    $scope.saveDirector = function () {
        if ($scope.DirectorModel.isActive == '1')
        { $scope.DirectorModel.isActive = true }
        else { $scope.DirectorModel.isActive = false }
        if ($scope.DirectorModel.Name.length == 0) return false;
        var status = 0;

        if ($scope.isEditDirector == true) {
            $http.post('/organization/Directors/editservice', {
                ID: $scope.DirectorModel.ID,
                Name: $scope.DirectorModel.Name.toUpperCase(),
                CompanyID: $scope.idx,
                NPWP : $('#directorNpwp').val(),//$scope.DirectorModel.NPWP,
                Status: $('#DirectorStatus').val(),
            })
              .success(function (data) {
                  $('#directorNpwp').val('');
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
                Name: $scope.DirectorModel.Name.toUpperCase(),
                CompanyID: $scope.idx,
                NPWP: $('#directorNpwp').val(),//$scope.DirectorModel.NPWP,
                Status: $('#DirectorStatus').val(),
            })
              .success(function (data) {
                  $('#directorNpwp').val('');
                  $scope.loadDirectors();
                  $scope.initDirectorModel();
              })
              .error(function () {
                  $scope.initDirectorModel();
              })
        }
        $scope.isEditDirector == false;
    }

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

    //end of DIRECTOR

    //SK AWAL
    $scope.isAddSkAwal = false;
    $scope.isEditSkAwal = false;
    $scope.isBtnAddSkAwalShowed = true;
    $scope.isBtnSaveSkAwalShowed = false;
    $scope.isBtnCancelSkAwalShowed = false;    

    $scope.tambahSkAwalClick = function () {
        $scope.isAddSkAwal = true;
        $scope.isBtnAddSkAwalShowed = false;
        $scope.isBtnSaveSkAwalShowed = true;
        $scope.isBtnCancelSkAwalShowed = true;

        if ($scope.isAddSkAwal) {
            
            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddSkAwal = true;
            $scope.isEditSkAwal = false;
        }
    }

    $scope.editSkAwalClick = function (id) {
        $http.get('/angkutjual/firstsks/findbyid/' + id)
        .success(function (data) {
            $scope.skAwalModel.id = data.ID;
            $scope.skAwalModel.nosurat = data.LetterNumber;
            // $scope.skAwalModel.tanggalpermohonan = data.LetterDate;
            $scope.skAwalModel.skFile = data.SkFile;
            $('#tanggalpermohonanskawal').val(data.LetterDate)            
            $('#tanggalpermohonanskawal').datepicker('update', data.LetterDate)
            $scope.skAwalModel.nosk = data.SkNumber;
            $scope.skAwalModel.tglsk = data.SkDate;
            $("#tglskawal").datepicker("update", data.SkDate);
            $scope.skAwalModel.tglberakhir = data.SkEndDate;
            $scope.skAwalModel.sertifikatCnc = data.SertifikatCNC;
            $('#tglberakhirskawal').datepicker('update', data.SkEndDate)
            $scope.skAwalModel.jangkawaktu = data.SkDuration;
            //ID: '-',
            //LetterNumber: $scope.skAwalModel.nosurat,
            //LetterDate: $('#tanggalpermohonanskawal').val(),// $scope.skAwalModel.tanggalpermohonan,
            //SkNumber: $scope.skAwalModel.nosk,
            //SkDate: $('#tglskawal').val(),// $scope.skAwalModel.tglsk,
            //SkEndDate: $('#tglberakhirskawal').val(),//$scope.skAwalModel.tglberakhir,
            //SkDuration: $scope.skAwalModel.jangkawaktu,

            $scope.isBtnAddSkAwalShowed = false;
            $scope.isBtnSaveSkAwalShowed = true;
            $scope.isBtnCancelSkAwalShowed = true;

            $scope.isAddSkAwal = false;
            $scope.isEditSkAwal = true;
        });
    }
    //Sumber Sk Perpanjangan

    $scope.loadSumberSkPerpanjangan = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSumberSkPerpanjangan/' + $scope.idx)
        .success(function (data) {
            $scope.dataSumberSkPerpanjangan = data.data;
        })
    }
    $scope.loadSumberSkPerpanjangan();
    $scope.tambahSumberSkPerpanjangan = function (id) {
        debugger;
        $scope.initSumberSkPerpanjanganModel();
        $scope.sumberSkPerpanjanganModel.ExtendedSkID = id;
        $scope.isEditSumberSkPerpanjangan = false;
        $('#addSumberSkPerpanjangan').modal('show');
    }
    $scope.simpanSumberSkPerpanjangan = function () {
        $http.post('/angkutjual/iupopangkutjual/CreateServiceSumberSkPerpanjangan', {
            CompanyName: $scope.sumberSkPerpanjanganModel.CompanyName.toUpperCase(),
            Volume: $scope.sumberSkPerpanjanganModel.Volume,
            ExtendedSkID: $scope.sumberSkPerpanjanganModel.ExtendedSkID,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $scope.loadSumberSkPerpanjangan();
        })
        .finally(function () {
            $scope.initSumberSkPerpanjanganModel();
            $('#addSumberSkPerpanjangan').modal('hide');
        })
    }
    $scope.editSumberSkPerpanjangan = function (id) {
        $http.get('/iupopangkutjual/FindSumberSkPerpanjangan/' + id)
        .success(function (data) {
            $scope.sumberSkPerpanjanganModel.ID = data.ID,
            $scope.sumberSkPerpanjanganModel.CompanyName = data.CompanyName,
            $scope.sumberSkPerpanjanganModel.Volume = data.Volume,
            $scope.sumberSkPerpanjanganModel.ExtendedSkID = data.ExtendedSkID,
            $scope.isEditSumberSkPerpanjangan = true;
            $('#addSumberSkPerpanjangan').modal('show');
        })
    }
    $scope.hapusSumberSkPerpanjangan = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/iupopangkutjual/DeleteSumberSkPerpanjangan', { id: id })
            .success(function (data) {
                $scope.loadSumberSkPerpanjangan();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.saveChangesSumberSkPerpanjangan = function (id) {
        debugger;
        $http.post('/IupOpAngkutJual/EditServiceSumberSkPerpanjangan/', {
            ID: $scope.sumberSkPerpanjanganModel.ID,
            CompanyName: $scope.sumberSkPerpanjanganModel.CompanyName.toUpperCase(),
            Volume: $scope.sumberSkPerpanjanganModel.Volume,
            ExtendedSkID: $scope.sumberSkPerpanjanganModel.ExtendedSkID,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $scope.loadSumberSkPerpanjangan();
        })
        .finally(function () {
            $scope.initSumberSkPerpanjanganModel();
            $('#addSumberSkPerpanjangan').modal('hide');
        })
    }


    //Sumber Perubahan Kerjasama
    $scope.loadSumberPerubahanKerjasama = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSumberPerubahanKerjasama/' + $scope.idx)
        .success(function (data) {
            $scope.dataSumberPerubahanKerjasama = data.data;
        })
    }
    $scope.loadSumberPerubahanKerjasama();

    $scope.tambahSumberPerubahan = function (id) {
        $scope.initSumberPerubahanKerjasamaModel();
        $scope.sumberPerubahanKerjasamaModel.SourceChangesID = id;
        $scope.isEditSumberPerubahanKerjasama = false;
        $('#addSumberPerubahanKerjasama').modal('show');
    }

    $scope.simpanSumberPerubahanKerjasama = function () {
        $http.post('/angkutjual/iupopangkutjual/CreateServiceSumberPerubahanKerjasama', {
            CompanyName: $scope.sumberPerubahanKerjasamaModel.CompanyName.toUpperCase(),
            Volume: $scope.sumberPerubahanKerjasamaModel.Volume,
            SourceChangesID : $scope.sumberPerubahanKerjasamaModel.SourceChangesID,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $scope.loadSumberPerubahanKerjasama();
        })
        .finally(function () {
            $scope.initSumberPerubahanKerjasamaModel();
            $('#addSumberPerubahanKerjasama').modal('hide');
        })
    }
    $scope.saveChangesSumberPerubahanKerjasama = function (id) {
        debugger;
        $http.post('/IupOpAngkutJual/EditServiceSumberPerubahanKerjasama/', {
            ID: $scope.sumberPerubahanKerjasamaModel.ID,
            CompanyName: $scope.sumberPerubahanKerjasamaModel.CompanyName.toUpperCase(),
            Volume: $scope.sumberPerubahanKerjasamaModel.Volume,
            SourceChangesID: $scope.sumberPerubahanKerjasamaModel.SourceChangesID,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            debugger;
            $scope.loadSumberPerubahanKerjasama();
        })
        .finally(function () {
            $scope.initSumberPerubahanKerjasamaModel();
            $('#addSumberPerubahanKerjasama').modal('hide');
        })
    }

    //editSumberPerubahanKerjasama
    $scope.editSumberPerubahanKerjasama = function (id) {
        $http.get('/iupopangkutjual/FindSumberPerubahanKerjasama/' + id)
        .success(function (data) {
            $scope.sumberPerubahanKerjasamaModel.ID = data.ID,
            $scope.sumberPerubahanKerjasamaModel.CompanyName = data.CompanyName,
            $scope.sumberPerubahanKerjasamaModel.Volume = data.Volume,
            $scope.sumberPerubahanKerjasamaModel.SourceChangesID = data.SourceChangesID,
            $scope.isEditSumberPerubahanKerjasama = true;
            $('#addSumberPerubahanKerjasama').modal('show');
        })
    }
    $scope.hapusSumberPerubahanKerjasama = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/iupopangkutjual/DeleteSumberPerubahanKerjasama', { id: id })
            .success(function (data) {
                $scope.loadSumberPerubahanKerjasama();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    //Sk Pengakhiran
    $scope.loadSkPengakhiran = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkPengakhiran/' + $scope.idx)
        .success(function (data) {
            $scope.dataSkPengakhiran = data.data;
        })
    }
    $scope.loadSkPengakhiran();
    $scope.tambahSkPengakhiran = function () {
        $('#skPengakhiranModal').modal('show');
        $scope.initSkPengakhiranModel();
        $('#tglmohon').val('');
        $('#tglSkPengakhiran').val('');
        $scope.isEditSkPengakhiran = false;
    }
    $scope.simpanSkPengakhiran = function () {
        $http.post('/iupopangkutjual/CreateServiceSimpanSkPengakhiran', {
            LetterNumber: $scope.skPengakhiranModel.LetterNumber,
            LetterDate: $('#tglmohon').data('datepicker').getFormattedDate('mm/dd/yyyy'),// $scope.perubahanKerjasamaModel.LetterDate,
            SkNumber: $scope.skPengakhiranModel.SkNumber,
            SkDate: $('#tglSkPengakhiran').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $scope.loadSkPengakhiran();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Pengakhiran';
            $scope.modelUpload.moduleType = '7'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initSkPengakhiranModel();
            $('#skPengakhiranModal').modal('hide');
            $scope.isEditSkPengakhiran = false;
        })
    }

    $scope.editSkPengakhiran = function (id) {
        $http.get('/iupopangkutjual/FindSkPengakhiran/' + id)
        .success(function (data) {
            $scope.skPengakhiranModel.ID = data.ID,
            $scope.skPengakhiranModel.LetterNumber = data.LetterNumber,
            $scope.skPengakhiranModel.LetterDate = data.LetterDate,
            $("#tglmohon").datepicker("update", data.LetterDate);
            $scope.skPengakhiranModel.SkNumber = data.SkNumber,
            $scope.skPengakhiranModel.SkDate = data.SkDate,
            $("#tglSkPengakhiran").datepicker("update", data.SkDate);
            $scope.skPengakhiranModel.CompanyId = data.CompanyId
            $scope.skPengakhiranModel.SkFile = data.SkFile
            $('#skPengakhiranModal').modal('show');
            $scope.isEditSkPengakhiran = true;
        })
    }
    $scope.saveChangesSkPengakhiran = function (id) {
        debugger;
        $http.post('/IupOpAngkutJual/EditServiceSkPengakhiran/', {
            ID: $scope.skPengakhiranModel.ID,
            LetterNumber: $scope.skPengakhiranModel.LetterNumber,
            LetterDate: $('#tglmohon').data('datepicker').getFormattedDate('mm/dd/yyyy'),// $scope.perubahanKerjasamaModel.LetterDate,
            SkNumber: $scope.skPengakhiranModel.SkNumber,
            SkFile: $scope.skPengakhiranModel.SkFile,
            SkDate: $('#tglSkPengakhiran').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            CompanyID: $scope.idx
        })
        .success(function (data) {
            debugger;
            $scope.loadSkPengakhiran();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Pengakhiran';
            $scope.modelUpload.moduleType = '7'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initSkPengakhiranModel();
            $('#skPengakhiranModal').modal('hide');
            $scope.isEditSkPengakhiran = false;
        })
    }

    $scope.hapusSkPengakhiran = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/iupopangkutjual/DeleteSkPengakhiran', { id: id })
            .success(function (data) {
                $scope.loadSkPengakhiran();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    //Sk Perpanjangan2

    $scope.loadSkPerpanjangan2 = function () {
        $http.get('/iupopangkutjual/LoadSkPerpanjangan2/' + $scope.idx)
        .success(function (data) {
            $scope.dataSkPerpanjangan = data.data;
        })
    }
    $scope.loadSkPerpanjangan2();

    $scope.simpanSkPerpanjangan2 = function () {
        $http.post('/iupopangkutjual/CreateServiceSimpanSkPerpanjangan2', {
            LetterNumber: $scope.skPerpanjanganModel.LetterNumber,
            LetterDate: $('#tglpermohonan').data('datepicker').getFormattedDate('mm/dd/yyyy'),// $scope.perubahanKerjasamaModel.LetterDate,
            SkNumber: $scope.skPerpanjanganModel.SkNumber,
            SkDate: $('#tglskawal2').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkDuration: $scope.skPerpanjanganModel.SkDuration,
            SkEndDate: $('#tglberakhirskawal2').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SertifikatCNC: $scope.skPerpanjanganModel.SertifikatCNC,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            $scope.loadSkPerpanjangan2();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Perpanjangan';
            $scope.modelUpload.moduleType = '3'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initSkPerpanjanganModel();
            $('#skPerpanjanganModal').modal('hide');
        })
    }
    

    $scope.editSkPerpanjangan2 = function (id) {
        $http.get('/iupopangkutjual/FindSkPerpanjangan2/' + id)
        .success(function (data) {
            $scope.skPerpanjanganModel.ID = data.ID,
            $scope.skPerpanjanganModel.LetterNumber = data.LetterNumber,
            $scope.skPerpanjanganModel.LetterDate = data.LetterDate,
            $("#tglpermohonan").datepicker("update", data.LetterDate);

            $scope.skPerpanjanganModel.SkNumber = data.SkNumber,
            $scope.skPerpanjanganModel.SkDate = data.SkDate,
            $("#tglskawal2").datepicker("update", data.SkDate);
            $('#tglberakhirskawal2').val(data.SkEndDate),
            $scope.skPerpanjanganModel.CompanyId = data.CompanyId
            $scope.skPerpanjanganModel.SertifikatCNC = data.SertifikatCNC
            $scope.skPerpanjanganModel.SkDuration = data.SkDuration
            $scope.skPerpanjanganModel.SkFile = data.SkFile
            $('#skPerpanjanganModal').modal('show');
            $scope.isEditSkPerpanjangan = true;
        })
    }
    //saveChangesSkPerpanjangan2
    $scope.saveChangesSkPerpanjangan2 = function (id) {
        debugger;
        $http.post('/IupOpAngkutJual/EditServiceSkPerpanjangan2/', {
            ID: $scope.skPerpanjanganModel.ID,
            LetterNumber: $scope.skPerpanjanganModel.LetterNumber,
            LetterDate: $('#tglpermohonan').data('datepicker').getFormattedDate('mm/dd/yyyy'),// $scope.perubahanKerjasamaModel.LetterDate,
            SkNumber: $scope.skPerpanjanganModel.SkNumber,
            SkDate: $('#tglskawal2').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkDuration: $scope.skPerpanjanganModel.SkDuration,
            SkEndDate: $('#tglberakhirskawal2').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SertifikatCNC: $scope.skPerpanjanganModel.SertifikatCNC,
            SkFile : $scope.skPerpanjanganModel.SkFile,
            CompanyID: $scope.idx
        })
        .success(function (data) {
            debugger;
            $scope.loadSkPerpanjangan2();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Perpanjangan';
            $scope.modelUpload.moduleType = '3'
            $scope.uploadFile();
        })
        .finally(function () {
            $scope.initSkPerpanjanganModel();
            $('#skPerpanjanganModal').modal('hide');
            $scope.isEditSkPerpanjangan = false;
        })
    }

    $scope.hapusSkPerpanjangan2 = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/iupopangkutjual/DeleteSkPerpanjangan2', { id: id })
            .success(function (data) {
                $scope.loadSkPerpanjangan2();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }

    $scope.tambahSkPerpanjangan = function () {
        $('#skPerpanjanganModal').modal('show');
        $scope.initSkPerpanjanganModel();
        $('#tglskawal2').val('');
        $('#tglpermohonan').val('');
        $('#tglberakhirskawal2').val('');
        $scope.isEditSkPerpanjangan = false;
    }

    //Perubahan Kerjasama
    $scope.tambahPerubahanKerjasama = function () {
        $('#perubahanKerjasamaModal').modal('show');
        $('#tanggalpermohonanperubahankerjasama').val('');
        $('#tglperubahankerjasama').val('');
    }
    //Edit Perubahan Kerjasama
    $scope.editPerubahanKerjasama = function (id) {
        $http.get('/iupopangkutjual/FindPerubahanKerjasama/' + id)
        .success(function (data) {
            $scope.perubahanKerjasamaModel.ID = data.ID,
            $scope.perubahanKerjasamaModel.LetterNumber = data.LetterNumber,
            $scope.perubahanKerjasamaModel.LetterDate = data.LetterDate,
            $("#tanggalpermohonanperubahankerjasama").datepicker("update", data.LetterDate);
            
            $scope.perubahanKerjasamaModel.SkNumber = data.SkNumber,
            $scope.perubahanKerjasamaModel.SkDate = data.SkDate,
            $("#tglperubahankerjasama").datepicker("update", data.SkDate);
            $('#tglperubahankerjasama').val(data.SkDate),
            $scope.perubahanKerjasamaModel.CompanyId = data.CompanyId
            $('#perubahanKerjasamaModal').modal('show');
            $scope.isAddPerubahanKerjasama = false;
            $scope.isEditPerubahanKerjasama = true;
        })
    }

    $scope.saveChangesPerubahanKerjasama = function (id) {
        debugger;
        $http.post('/IupOpAngkutJual/EditServicePerubahanKerjasama/', {
            ID: $scope.perubahanKerjasamaModel.ID,
            LetterNumber: $scope.perubahanKerjasamaModel.LetterNumber,
            LetterDate: $('#tanggalpermohonanperubahankerjasama').data('datepicker').getFormattedDate('mm/dd/yyyy'),// $scope.perubahanKerjasamaModel.LetterDate,
            SkNumber: $scope.perubahanKerjasamaModel.SkNumber,
            SkDate: $('#tglperubahankerjasama').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            CompanyID: $scope.idx
        })
        .success(function (data) {
            debugger;
            $scope.loadPerubahanKerjasama();
        })
        .finally(function () {
            $scope.initPerubahanKerjasamaModel();
            $('#perubahanKerjasamaModal').modal('hide');
        })
    }

    $scope.hapusPerubahanKerjasama = function (id) {
        swal({
            title: "Delete Data", text: "Hapus Data? ",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/iupopangkutjual/DeletePerubahanKerjasama', { id: id })
            .success(function (data) {
                $scope.loadPerubahanKerjasama();
                swal("Sukses!", "Data telah dihapus", "success");
            })
        });
    }
    
    $scope.simpanPerubahanKerjasama = function () {
        $http.post('/iupopangkutjual/CreateServicePerubahanKerjasama', {
            LetterNumber: $scope.perubahanKerjasamaModel.LetterNumber,
            LetterDate: $('#tanggalpermohonanperubahankerjasama').data('datepicker').getFormattedDate('mm/dd/yyyy'),// $scope.perubahanKerjasamaModel.LetterDate,
            SkNumber: $scope.perubahanKerjasamaModel.SkNumber,
            SkDate: $('#tglperubahankerjasama').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            CompanyID: $scope.idx
        })
        .success(function (data) {
            //$scope.loadSkBkpmIupOp();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Bkpm';
            //$scope.modelUpload.moduleType = '10'
            //$('#addSkBkpmIupOp').modal('hide');
            //$scope.uploadFile();
            //$scope.loadPeopleInvolved();
            $scope.loadPerubahanKerjasama();
        })
        .finally(function () {
            $scope.initPerubahanKerjasamaModel();
            $('#perubahanKerjasamaModal').modal('hide');
        })
    }

    $scope.loadPerubahanKerjasama = function () {
        $http.get('/iupopangkutjual/LoadPerubahanKerjasama/' + $scope.idx)
        .success(function (data) {
            $scope.dataPerubahanKerjasama = data.data;
        })
    }
    $scope.loadPerubahanKerjasama();

    $scope.cancelTambahEditSkAwalClick = function () {
        $scope.isAddSkAwal = false;
        $scope.isEditSkAwal = false;

        $scope.isBtnAddSkAwalShowed = true;
        $scope.isBtnSaveSkAwalShowed = false;
        $scope.isBtnCancelSkAwalShowed = false;

    }


    $scope.saveTambahEditSkAwalClick = function () {
        debugger;
        if ($scope.isAddSkAwal == true) {
            $scope.saveSkAwal();
        }
        else if ($scope.isEditSkAwal == true) {
            $scope.saveChangesSkAwal();
        }
        $scope.isAddSkAwal = false;
        $scope.isEditSkAwal = false;

        $scope.isBtnAddSkAwalShowed = true;
        $scope.isBtnSaveSkAwalShowed = false;
        $scope.isBtnCancelSkAwalShowed = false;
    }

    $scope.loadSkAwal = function () {
        $http.get('/angkutjual/iupopangkutjual/loadskawal/' + $scope.idx)
        .success(function (data) {
            $scope.skAwal = data.skAwal;
            $timeout(function () {
                $scope.loadSkAwalIupOp();
                $scope.loadSkAwalTujuanPenjualan();
            }, 2000)

        })
        .finally(function () { })
    }
    $scope.loadSkAwal();

    $scope.saveSkAwal = function () {
        /*  nosurat: '',
            tanggalpermohonan: '',
            nosk: '',
            tglsk: '',
            tglberakhir: '',
            jangkawaktu: '',
            fileSk: ''*/
        $http.post('/angkutjual/firstsks/createservice', {
            ID: '-',
            LetterNumber: $scope.skAwalModel.nosurat,
            LetterDate: $('#tanggalpermohonanskawal').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkNumber: $scope.skAwalModel.nosk,
            SkDate: $('#tglskawal').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkEndDate: $('#tglberakhirskawal').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkDuration: $scope.skAwalModel.jangkawaktu,
            SertifikatCNC: $scope.skAwalModel.sertifikatCnc,
            // AdditionalInfo: $scope.skAwalModel.isActive,
            // SkFile: $scope.skAwalModel.fileSk,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            debugger;
            $scope.loadSkAwal();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK AWAL';
            $scope.modelUpload.moduleType = '1'           
            $scope.uploadFile();

        })
        .error(function () {
        })
        .finally(function () {
            $scope.initShareholderModel();
        })
    }
    $scope.saveChangesSkAwal = function () {
        debugger;
        /*  nosurat: '',
            tanggalpermohonan: '',
            nosk: '',
            tglsk: '',
            tglberakhir: '',
            jangkawaktu: '',
            fileSk: ''*/
        $http.post('/angkutjual/firstsks/editservice', {
            ID: $scope.skAwalModel.id,
            LetterNumber: $scope.skAwalModel.nosurat,
            LetterDate: $('#tanggalpermohonanskawal').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkNumber: $scope.skAwalModel.nosk,
            SkDate: $('#tglskawal').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkEndDate: $('#tglberakhirskawal').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            SkDuration: $scope.skAwalModel.jangkawaktu,
            SertifikatCNC: $scope.skAwalModel.sertifikatCnc,
            // AdditionalInfo: $scope.skAwalModel.isActive,
            // SkFile: $scope.skAwalModel.fileSk,
            CompanyID: $scope.idx,
            SkFile : $scope.skAwalModel.skFile
        })
        .success(function (data) {
            $scope.loadSkAwal();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK AWAL';
            $scope.modelUpload.moduleType = '1'
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initShareholderModel();
        })
    }

    $scope.editSkAwalIupOp = function (id) {
        debugger;
        $scope.isEditSkAwalIupOp = true;
        $http.get('/angkutjual/iupopangkutjual/FindFirstSkSource/' + id)
        .success(function (data) {
            $scope.FirstSkSourceModel.ID = data.ID;
            $scope.FirstSkSourceModel.CompanyName = data.CompanyName;
            $scope.FirstSkSourceModel.Volume = data.Volume;
            
            //$scope.firstSkSource.FirstSkID = data.FirstSkID;
            $('#addSkIupOp').modal('show');
        });
    }
    

    $scope.tambahSkAwalIupOp = function () {
        $scope.initSkAwalIupOpModel();
        $scope.isEditSkAwalIupOp = false;
        $('#addSkIupOp').modal('show');
    }

    $scope.isEditSkAwalIupOp = false;
    //$scope.editSkAwalIupOp = function (id) {
    //    $scope.isEditSkAwalIupOp = true;
    //    $http.get('/angkutjual/coalsourcesks/findbyid/' + id)
    //    .success(function (data) {
    //        $scope.skAwalIupModel.id = data.ID;
    //        $scope.skAwalIupModel.skNumber = data.SkNumber;
    //        $scope.skAwalIupModel.skDate = data.SkDate;
    //        $scope.skAwalIupModel.skFile = data.SkFile;
    //        $scope.skAwalIupModel.companySource = data.CompanySource;
    //        $scope.skAwalIupModel.companySourceAddress = data.CompanySourceAddress;
    //        $scope.skAwalIupModel.city = data.City;
    //        $scope.skAwalIupModel.province = data.Province;
    //        $scope.skAwalIupModel.tonnage = data.Tonnage;
    //        console.log($scope.skAwalIupModel.tonnage)
    //        $('#addSkIupOp').modal('show');
    //    });        
    //}

    //$scope.simpanSkAwalIupOp = function () {
    //    $http.post('/angkutjual/coalsourcesks/createservice', {
    //        SkNumber: $scope.skAwalIupModel.skNumber,
    //        SkDate: $scope.skAwalIupModel.skDate,
    //        SkFile: $scope.skAwalIupModel.skFile,
    //        CompanySource: $scope.skAwalIupModel.companySource,
    //        CompanySourceAddress: $scope.skAwalIupModel.companySourceAddress,
    //        City: $scope.skAwalIupModel.city,
    //        Province: $scope.skAwalIupModel.province,
    //        SalesDestination: $scope.skAwalIupModel.salesDestination,
    //        Remark: $scope.skAwalIupModel.remark,
    //        Tonnage: $scope.skAwalIupModel.tonnage,
    //        firstSkID: $('#skAwalId').val()

    //    })
    //    .success(function (data) {
    //        $scope.loadSkAwalIupOp();
    //        $scope.modelUpload.id = data;
    //        $scope.modelUpload.title = 'IUP OP SK Awal';
    //        $scope.modelUpload.moduleType = '2'
    //         $('#addSkIupOp').modal('hide');
    //        $scope.uploadFile();
          
    //    })
    //    .finally(function () { })
    //}
    $scope.simpanSkAwalIupOp = function () {
        $http.post('/angkutjual/iupopangkutjual/CreateServiceFirstSkSource', {
            CompanyName: $scope.FirstSkSourceModel.CompanyName.toUpperCase(),
            Volume: $scope.FirstSkSourceModel.Volume,
            FirstSkID: $('#skAwalId').val()
        })
        .success(function (data) {
            $scope.loadSkAwalIupOp();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'Sumber';
            //$scope.modelUpload.moduleType = '2'
            $('#addSkIupOp').modal('hide');
            //$scope.uploadFile();
        })
        .finally(function () { })
    }

    $scope.simpanEditSkAwalIupOp = function () {
        $http.post('/angkutjual/iupopangkutjual/EditServiceFirstSkSource', {
            ID : $scope.FirstSkSourceModel.ID,
            CompanyName: $scope.FirstSkSourceModel.CompanyName.toUpperCase(),
            Volume: $scope.FirstSkSourceModel.Volume,
            FirstSkID: $('#skAwalId').val()
        })
        .success(function (data) {
            $scope.loadSkAwalIupOp();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
            $('#addSkIupOp').modal('hide');
            //$scope.uploadFile();
            $scope.isEditSkAwalIupOp = false;

        })
        .finally(function () { })
    }

    $scope.loadSkAwalIupOp = function () {
        $http.get('/angkutjual/iupopangkutjual/loadiupopskawal/' + $('#skAwalId').val())
        .success(function (data) {
            $scope.firstSkSource = data.data;
        })
    }
    $scope.loadSkAwalIupOp();
    
    $scope.hapusSkAwalIupOp = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
           
            $http.post('/angkutjual/iupopangkutjual/DeleteServiceFirstSkSource', { id: id })
            .success(function (data) {
                $scope.loadSkAwalIupOp();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    $scope.tambahSkAwalTujuanPenjualan = function () {
        $scope.initSalesDestinationModel();
        $('#addSalesDestination').modal('show');
    }
    $scope.editSkAwalTujuanPenjualan = function () {
        $scope.initSalesDestinationModel();
        $('#addSalesDestination').modal('show');
    }
    
    $scope.simpanSkAwalTujuanPenjualan = function () {
        $http.post('/angkutjual/salesdestinations/createservice', {
            Destination: $scope.salesDestinationModel.Destination,
            Remark: $scope.salesDestinationModel.Remark,
            firstSkID: $('#skAwalId').val()

        })
        .success(function (data) {
            $scope.loadSkAwalTujuanPenjualan();
        })
        .finally(function () { })
    }

    $scope.simpanEditSkAwalTujuanPenjualan = function () {
        $http.post('/angkutjual/salesdestinations/createservice', {
            Destination: $scope.salesDestinationModel.Destination,
            Remark: $scope.salesDestinationModel.Remark,
            firstSkID: $('#skAwalId').val()

        })
        .success(function (data) {
            $scope.loadSkAwalTujuanPenjualan();
        })
        .finally(function () { })
    }

    $scope.loadSkAwalTujuanPenjualan = function () {
        $http.get('/angkutjual/salesdestinations/LoadSalesDestination/' + $('#skAwalId').val())
        .success(function (data) {
            $scope.salesdestinations = data.data;
        })
    }
    $scope.hapusSkAwalTujuanPenjualan = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/salesdestinations/deleteserivice', { id: id })
            .success(function (data) {
                $scope.loadSkAwalTujuanPenjualan();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }





    //END OF SK AWAL



    //SK PERPANJANGAN

    $scope.isAddSkPerpanjangan = false;
    $scope.isEditSkPerpanjangan = false;
    $scope.isBtnAddSkPerpanjanganShowed = true;
    $scope.isBtnSaveSkPerpanjanganShowed = false;
    $scope.isBtnCancelSkPerpanjanganShowed = false;
    
    $scope.tambahSkPerpanjanganClick = function () {

        $scope.isBtnAddSkPerpanjanganShowed = false;
        $scope.isBtnSaveSkPerpanjanganShowed = true;
        $scope.isBtnCancelSkPerpanjanganShowed = true;

        $scope.isAddSkPerpanjangan = true;
        $scope.isEditSPerpanjangan = false;
    }


    $scope.editSkPerpanjanganClick = function (id) {
        $http.get('/angkutjual/extendedsks/findbyid/' + id)
        .success(function (data) {
            $scope.skPerpanjanganModel.id = data.ID;
            $scope.skPerpanjanganModel.LetterNumber = data.LetterNumber;
            $scope.skPerpanjanganModel.skFile = data.SkFile;

            
            $scope.skPerpanjanganModel.LetterDate = data.LetterDate;
            $('#tanggalpermohonanskperpanjangan').val(data.LetterDate)
            $('#tanggalpermohonanskperpanjangan').datepicker('update', data.LetterDate)

            $scope.skPerpanjanganModel.SkNumber = data.SkNumber;

            $scope.skPerpanjanganModel.SkDate = data.SkDate;
            $('#tglskperpanjangan').val(data.SkDate);
            $('#tglskperpanjangan').datepicker('update', data.SkDate)

            $scope.skPerpanjanganModel.SkEndDate = data.SkEndDate;
            $('#tglberakhirskperpanjangan').val(data.SkDate);
            $('#tglberakhirskperpanjangan').datepicker('update', data.SkEndDate)

            $scope.skPerpanjanganModel.SkDuration = data.SkDuration;

            $scope.skPerpanjanganModel.RpiitNumber = data.RpiitNumber;

            $scope.skPerpanjanganModel.RpiitDate = data.RpiitDate;
            $('#tanggalrpiitskperpanjangan').val(data.RpiitDate);
            $('#tanggalrpiitskperpanjangan').datepicker('update', data.RpiitDate);



            $scope.isBtnAddSkPerpanjanganShowed = false;
            $scope.isBtnSaveSkPerpanjanganShowed = true;
            $scope.isBtnCancelSkPerpanjanganShowed = true;

            $scope.isAddSkPerpanjangan = false;
            $scope.isEditSkPerpanjangan = true;
        });
    }


    $scope.cancelTambahEditSkPerpanjanganClick = function () {
        $scope.isAddSkPerpanjangan = false;
        $scope.isEditSkPerpanjangan = false;

        $scope.isBtnAddSkPerpanjanganShowed = true;
        $scope.isBtnSaveSkPerpanjanganShowed = false;
        $scope.isBtnCancelSkPerpanjanganShowed = false;

    }

    $scope.saveTambahEditSkPerpanjanganClick = function () {

        if ($scope.isAddSkPerpanjangan == true) {
            $scope.saveSkPerpanjangan();
        }
        else if ($scope.isEditSkPerpanjangan == true) {

            $scope.saveChangesSkPerpanjangan();
        }


        $scope.isAddSkPerpanjangan = false;
        $scope.isEditSkPerpanjangan = false;

        $scope.isBtnAddSkPerpanjanganShowed = true;
        $scope.isBtnSaveSkPerpanjanganShowed = false;
        $scope.isBtnCancelSkPerpanjanganShowed = false;

    }

    $scope.loadSkPerpanjangan = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkPerpanjangan/' + $scope.idx)
        .success(function (data) {
            $scope.skPerpanjangan = data.skPerpanjangan;
            $timeout(function () {
                $scope.loadSkPerpanjanganIupOp();
                $scope.loadSkPerpanjanganTujuanPenjualan();
            }, 3000)

        })
        .finally(function () { })
    }
    $scope.loadSkPerpanjangan();

    $scope.saveSkPerpanjangan = function () {

        /*     LetterNumber: '',
            LetterDate: '',
            RpiitNumber: '',
            RpiitDate: '',
            SkNumber: '',
            SkDate: '',
            SkEndDate: '',
            SkDuration: '',*/
        $http.post('/angkutjual/ExtendedSks/createservice', {
            ID: '-',
            LetterNumber: $scope.skPerpanjanganModel.LetterNumber,
            LetterDate: $('#tanggalpermohonanskperpanjangan').val(),
            SkNumber: $scope.skPerpanjanganModel.SkNumber,
            SkDate: $('#tglskperpanjangan').val(),
            SkEndDate: $('#tglberakhirskperpanjangan').val(),//$scope.skAwalModel.tglberakhir,
            SkDuration: $scope.skPerpanjanganModel.SkDuration,
            RpiitNumber: $scope.skPerpanjanganModel.RpiitNumber,
            RpiitDate: $('#tanggalrpiitskperpanjangan').val(),
            CompanyID: $scope.idx,
           
        })
        .success(function (data) {
            $scope.loadSkPerpanjangan();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Perpanjangan';
            $scope.modelUpload.moduleType = '3'
            $('#addSkIupOp').modal('hide');
            $scope.uploadFile();
           
        })
        .error(function () {

        })
        .finally(function () {
            $scope.skPerpanjanganModel();
        })
    }

    $scope.saveChangesSkPerpanjangan = function () {

        /*     LetterNumber: '',
           LetterDate: '',
           RpiitNumber: '',
           RpiitDate: '',
           SkNumber: '',
           SkDate: '',
           SkEndDate: '',
           SkDuration: '',*/
        $http.post('/angkutjual/ExtendedSks/editservice', {
            ID: $scope.skPerpanjanganModel.id,
            LetterNumber: $scope.skPerpanjanganModel.LetterNumber,
            LetterDate: $('#tanggalpermohonanskperpanjangan').val(),
            SkNumber: $scope.skPerpanjanganModel.SkNumber,
            SkDate: $('#tglskperpanjangan').val(),
            SkEndDate: $('#tglberakhirskperpanjangan').val(),
            RpiitNumber: $scope.skPerpanjanganModel.RpiitNumber,
            RpiitDate: $('#tanggalrpiitskperpanjangan').val(),
            CompanyID: $scope.idx,
            SkDuration: $scope.skPerpanjanganModel.SkDuration,
            skFile : $scope.skPerpanjanganModel.skFile 
        })
        .success(function (data) {
            $scope.loadSkPerpanjangan();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Perpanjangan';
            $scope.modelUpload.moduleType = '3'
            $('#addSkIupOp').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initShareholderModel();
        })
    }

    $scope.tambahSkPerpanjanganIupOp = function () {
        $scope.initSkPerpanjanganIupOpModel();
        $('#addSkPerpanjanganIupOp').modal('show');
    }

    $scope.simpanSkPerpanjanganIupOp = function () {
        $http.post('/angkutjual/coalsourcesks/createservice', {
            SkNumber: $scope.skPerpanjanganIupModel.skNumber,
            SkDate: $scope.skPerpanjanganIupModel.skDate,
            SkFile: $scope.skPerpanjanganIupModel.skFile,
            CompanySource: $scope.skPerpanjanganIupModel.companySource,
            CompanySourceAddress: $scope.skPerpanjanganIupModel.companySourceAddress,
            City: $scope.skPerpanjanganIupModel.city,
            Province: $scope.skPerpanjanganIupModel.province,
            SalesDestination: $scope.skPerpanjanganIupModel.salesDestination,
            Remark: $scope.skPerpanjanganIupModel.remark,
            Tonnage: $scope.skPerpanjanganIupModel.tonnage,
            ExtendedSkID: $('#skPerpanjanganId').val()

        })
        .success(function (data) {

            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'IUP OP SK Perpanjangan';
            $scope.modelUpload.moduleType = '4'
            $('#addSkPerpanjanganIupOp').modal('hide');
            $scope.uploadFile();


        })
        .finally(function () { })
    }

    $scope.loadSkPerpanjanganIupOp = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadIupOpSkPerpanjangan/' + $('#skPerpanjanganId').val())
        .success(function (data) {
            $scope.coalsourcesksperpanjangan = data.data;
        })
    }

    $scope.hapusSkPerpanjanganIupOp = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/coalsourcesks/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadSkPerpanjanganIupOp();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }



    $scope.tambahSkPerpanjanganTujuanPenjualan = function () {
        $scope.initSalesDestinationSkPerpanjanganModel();
        $('#addSalesDestinationPerpanjangan').modal('show');
    }
    $scope.simpanSkPerpanjanganTujuanPenjualan = function () {
        $http.post('/angkutjual/salesdestinations/createservice', {
            Destination: $scope.salesDestinationSkPerpanjanganModel.Destination,
            Remark: $scope.salesDestinationSkPerpanjanganModel.Remark,
            ExtendedSkID: $('#skPerpanjanganId').val()

        })
        .success(function (data) {
            $scope.loadSkPerpanjanganTujuanPenjualan();
        })
        .finally(function () { })
    }
    $scope.loadSkPerpanjanganTujuanPenjualan = function () {
        $http.get('/angkutjual/salesdestinations/LoadSalesDestinationPerpanjangan/' + $('#skPerpanjanganId').val())
        .success(function (data) {
            $scope.salesdestinationsperpanjangan = data.data;
        })
    }
    $scope.hapusSkPerpanjanganTujuanPenjualan = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/salesdestinations/deleteserivice', { id: id })
            .success(function (data) {
                $scope.loadSkPerpanjanganTujuanPenjualan();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }


    //END OF SK PERPANJANGAN



    //SK PENYESUAIAN

    $scope.isAddSkPenyesuaian = false;
    $scope.isEditSkPenyesuaian = false;
    $scope.isBtnAddSkPenyesuaianShowed = true;
    $scope.isBtnSaveSkPenyesuaianShowed = false;
    $scope.isBtnCancelSkPenyesuaianShowed = false;

    $scope.tambahSkPenyesuaianClick = function () {

        $scope.initSkPenyesuaianModel();
        $('#addSkPenyesuaian').modal('show');


        $scope.isBtnAddSkaddSkPenyesuaianShowed = false;
        $scope.isBtnSaveSkaddSkPenyesuaianShowed = true;
        $scope.isBtnCancelSkaddSkPenyesuaianShowed = true;

        if ($scope.isAddSkaddSkPenyesuaian) {

            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddSkaddSkPenyesuaian = true;
            $scope.isEditSkPenyesuaian = false;

        }
    }

    $scope.cancelTambahEditSkPenyesuaianClick = function () {
        $scope.isAddSkPenyesuaian = false;
        $scope.isEditSkPenyesuaian = false;

        $scope.isBtnAddSkPenyesuaianShowed = true;
        $scope.isBtnSaveSkPenyesuaianShowed = false;
        $scope.isBtnCancelSkPenyesuaianShowed = false;

    }


    $scope.saveTambahEditSkPenyesuaianClick = function () {

        $scope.saveSkPenyesuaian();

        $scope.isAddSkPenyesuaian = false;
        $scope.isEditSkPenyesuaian = false;

        $scope.isBtnAddSkPenyesuaianShowed = true;
        $scope.isBtnSaveSkPenyesuaianShowed = false;
        $scope.isBtnCancelSkPenyesuaianShowed = false;

    }

    $scope.loadSkPenyesuaian = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkPenyesuaian/' + $scope.idx)
        .success(function (data) {
            $scope.skPenyesuaian = data.skPenyesuaian;
            //$timeout(function () {
            //    $scope.loadSkAwalIupOp();
            //    $scope.loadSkAwalTujuanPenjualan();
            //}, 2000)

        })
        .finally(function () { })
    }
    $scope.loadSkPenyesuaian();

    $scope.saveSkPenyesuaian = function () {

       
        $http.post('/angkutjual/adjustedsks/createservice', {
            ID: '-',
            LetterNumber: $scope.skPenyesuaianModel.LetterNumber,
            LetterDate: $('#tanggalpermohonanskpenyesuaian').val(),// $scope.skAwalModel.tanggalpermohonan,
            SkNumber: $scope.skPenyesuaianModel.SkNumber,
            SkDate: $('#tglskpenyesuaian').val(),// $scope.skAwalModel.tglsk,
            RpiitNumber: $scope.skPenyesuaianModel.RpiitNumber,
            RpiitDate: $('#tglrpiitskpenyesuaian').val(),//$scope.skAwalModel.tglberakhir,            
            AdditionalInfo: $scope.skPenyesuaianModel.SkAdditionalInfo,
            // SkFile: $scope.skAwalModel.fileSk,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadSkPenyesuaian();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK PENYESUAIAN';
            $scope.modelUpload.moduleType = '5';
            $('#addSkPenyesuaian').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initSkPenyesuaianModel();
        })
    }

    $scope.hapusSkPenyesuaian = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/AdjustedSks/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadSkPenyesuaian();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    //END OF SK PENYESUAIAN




    //SK PENGHENTIAN SEMENTARA

    $scope.isAddSkPenghentianSementara = false;
    $scope.isEditSkPenghentianSementara = false;
    $scope.isBtnAddSkPenghentianSementaraShowed = true;
    $scope.isBtnSaveSkPenghentianSementaraShowed = false;
    $scope.isBtnCancelSkPenghentianSementaraShowed = false;

    $scope.tambahSkPenghentianSementaraClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addSkPenghentianSementara').modal('show');


        $scope.isBtnAddSkPenghentianSementaraShowed = false;
        $scope.isBtnSaveSkPenghentianSementaraShowed = true;
        $scope.isBtnCancelSkPenghentianSementaraShowed = true;

        if ($scope.isAddSkPenghentianSementara) {

            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddSkPenghentianSementara = true;
            $scope.isEditSkPenghentianSementara = false;

        }
    }

    $scope.cancelTambahEditSkPenghentianSementaraClick = function () {
        $scope.isAddSkPenghentianSementara = false;
        $scope.isEditSkPenghentianSementara = false;

        $scope.isBtnAddSkPenghentianSementaraShowed = true;
        $scope.isBtnSaveSkPenghentianSementaraShowed = false;
        $scope.isBtnCancelSkPenghentianSementaraShowed = false;

    }


    $scope.saveTambahEditSkPenghentianSementaraClick = function () {

        $scope.saveSkPenghentianSementara();

        $scope.isAddSkPenghentianSementara = false;
        $scope.isEditSkPenghentianSementara = false;

        $scope.isBtnAddSkPenghentianSementaraShowed = true;
        $scope.isBtnSaveSkPenghentianSementaraShowed = false;
        $scope.isBtnCancelSkPenghentianSementaraShowed = false;

    }

    $scope.loadSkPenghentianSementara = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkPenghentianSementara/' + $scope.idx)
        .success(function (data) {
            $scope.skPenghentianSementara = data.skPenghentianSementara;
            
        })
        .finally(function () { })
    }
    $scope.loadSkPenghentianSementara();

    $scope.saveSkPenghentianSementara = function () {


        $http.post('/angkutjual/tempbannedsks/createservice', {
            ID: '-',           
            SkNumber: $scope.skPenghentianSementaraModel.SkNumber,
            SkDate: $('#tglskpenghentiansementara').val(),// $scope.skAwalModel.tglsk,      
            AdditionalInfo: $scope.skPenghentianSementaraModel.SkAdditionalInfo,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadSkPenghentianSementara();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Penghentian Sementara';
            $scope.modelUpload.moduleType = '6';
            $('#addSkPenghentianSementara').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initSkPenghentianSementaraModel();
        })
    }

    $scope.hapusSkPenghentianSementara = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/tempbannedsks/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadSkPenghentianSementara();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //END OF SK PENGHENTIAN SEMENTARA



    //SK PENCABUTAN IJIN

    $scope.isAddSkPencabutanIjin = false;
    $scope.isEditSkPencabutanIjin = false;
    $scope.isBtnAddSkPencabutanIjinShowed = true;
    $scope.isBtnSaveSkPencabutanIjinShowed = false;
    $scope.isBtnCancelSkPencabutanIjinShowed = false;

    $scope.tambahSkPencabutanIjinClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addSkPencabutanIjin').modal('show');

        $scope.isBtnAddSkPencabutanIjinShowed = false;
        $scope.isBtnSaveSkPencabutanIjinShowed = true;
        $scope.isBtnCancelSkPencabutanIjinShowed = true;

        if ($scope.isAddSkPencabutanIjin) {

            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddSkPencabutanIjin = true;
            $scope.isEditSkPencabutanIjin = false;

        }
    }

    $scope.cancelTambahEditSkPencabutanIjinClick = function () {
        $scope.isAddSkPencabutanIjin = false;
        $scope.isEditSkPencabutanIjin = false;

        $scope.isBtnAddSkPencabutanIjinShowed = true;
        $scope.isBtnSaveSkPencabutanIjinShowed = false;
        $scope.isBtnCancelSkPencabutanIjinShowed = false;

    }
    
    $scope.saveTambahEditSkPencabutanIjinClick = function () {

        $scope.saveSkPencabutanIjin();

        $scope.isAddSkPencabutanIjin = false;
        $scope.isEditSkPencabutanIjin = false;

        $scope.isBtnAddSkPencabutanIjinShowed = true;
        $scope.isBtnSaveSkPencabutanIjinShowed = false;
        $scope.isBtnCancelSkPencabutanIjinShowed = false;

    }

    $scope.loadSkPencabutanIjin = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkPencabutanIjin/' + $scope.idx)
        .success(function (data) {
            $scope.skPencabutanIjin = data.skPencabutanIjin;

        })
        .finally(function () { })
    }
    $scope.loadSkPencabutanIjin();

    $scope.saveSkPencabutanIjin = function () {

        $http.post('/angkutjual/BannedSks/createservice', {
            ID: '-',
            SkNumber: $scope.skPencabutanIjinModel.SkNumber,
            SkDate: $('#tglskPencabutanIjin').val(),// $scope.skAwalModel.tglsk,      
            AdditionalInfo: $scope.skPencabutanIjinModel.SkAdditionalInfo,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadSkPencabutanIjin();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Pencabutan Ijin';
            $scope.modelUpload.moduleType = '7';
            $('#addSkPencabutanIjin').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initSkPencabutanIjinModel();
        })
    }

    $scope.hapusSkPencabutanIjin = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/BannedSks/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadSkPencabutanIjin();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //END OF SK PENCABUTAN IJIN



    //SK PENGAKTIFAN KEMBALI

    $scope.isAddSkPengaktifan = false;
    $scope.isEditSkPengaktifan = false;
    $scope.isBtnAddSkPengaktifanShowed = true;
    $scope.isBtnSaveSkPengaktifanShowed = false;
    $scope.isBtnCancelSkPengaktifanShowed = false;

    $scope.tambahSkPengaktifanClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addSkPengaktifan').modal('show');

        $scope.isBtnAddSkPengaktifanShowed = false;
        $scope.isBtnSaveSkPengaktifanShowed = true;
        $scope.isBtnCancelSkPengaktifanShowed = true;

        if ($scope.isAddSkPengaktifan) {

            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddSkPengaktifan = true;
            $scope.isEditSkPengaktifan = false;

        }
    }

    $scope.cancelTambahEditSkPengaktifanClick = function () {
        $scope.isAddSkPengaktifan = false;
        $scope.isEditSkPengaktifan = false;

        $scope.isBtnAddSkPengaktifanShowed = true;
        $scope.isBtnSaveSkPengaktifanShowed = false;
        $scope.isBtnCancelSkPengaktifanShowed = false;

    }
    
    $scope.saveTambahEditSkPengaktifanClick = function () {

        $scope.saveSkPengaktifan();

        $scope.isAddSkPengaktifan = false;
        $scope.isEditSkPengaktifan = false;

        $scope.isBtnAddSkPengaktifanShowed = true;
        $scope.isBtnSaveSkPengaktifanShowed = false;
        $scope.isBtnCancelSkPengaktifanShowed = false;

    }

    $scope.loadSkPengaktifan = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkPengaktifan/' + $scope.idx)
        .success(function (data) {
            $scope.skPengaktifan = data.skPengaktifan;

        })
        .finally(function () { })
    }
    $scope.loadSkPengaktifan();

    $scope.saveSkPengaktifan = function () {


        $http.post('/angkutjual/ReactivatedSks/createservice', {
            ID: '-',
            SkNumber: $scope.skPengaktifanModel.SkNumber,
            SkDate: $('#tglskPengaktifan').val(),// $scope.skAwalModel.tglsk,      
            AdditionalInfo: $scope.skPengaktifanModel.SkAdditionalInfo,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadSkPengaktifan();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK Pencabutan Ijin';
            $scope.modelUpload.moduleType = '8';
            $('#addSkPengaktifan').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initSkPengaktifanModel();
        })
    }

    $scope.hapusSkPengaktifan = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/ReactivatedSks/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadSkPengaktifan();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //END OF SK PENGAKTIFAN KEMBALI

    




    //SK  BKPM

    $scope.isAddSkBkpm = false;
    $scope.isEditSkBkpm = false;
    $scope.isBtnAddSkBkpmShowed = true;
    $scope.isBtnSaveSkBkpmShowed = false;
    $scope.isBtnCancelSkBkpmShowed = false;

    $scope.tambahSkBkpmClick = function () {

        $scope.isBtnAddSkBkpmShowed = false;
        $scope.isBtnSaveSkBkpmShowed = true;
        $scope.isBtnCancelSkBkpmShowed = true;

        if ($scope.isAddSkBkpm) {

            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddSkBkpm = true;
            $scope.isEditSBkpm = false;

        }
    }


    $scope.editSkBkpmClick = function (id) {


        $http.get('/angkutjual/BKPMs/findbyid/' + id)
        .success(function (data) {
            $scope.skBkpmModel.id = data.ID;
            $scope.skBkpmModel.LetterNumber = data.LetterNumber;
            $scope.skBkpmModel.SKFile = data.SKFile;

            $scope.skBkpmModel.LetterType = data.LetterType;
            $scope.skBkpmModel.LetterDate = data.LetterDate;
            $('#skbkpmletterdate').val(data.LetterDate)
            $('#skbkpmletterdate').datepicker('update', data.LetterDate)

            $scope.skBkpmModel.SkNumber = data.SkNumber;

            $scope.skBkpmModel.BKPMAcceptanceDate = data.BKPMAcceptanceDate;
            $('#BKPMAcceptanceDate').val(data.BKPMAcceptanceDate);
            $('#BKPMAcceptanceDate').datepicker('update', data.BKPMAcceptanceDate)


            $scope.skBkpmModel.EvaluatorAcceptanceDate = data.EvaluatorAcceptanceDate;
            $('#EvaluatorAcceptanceDate').val(data.EvaluatorAcceptanceDate);
            $('#EvaluatorAcceptanceDate').datepicker('update', data.EvaluatorAcceptanceDate)

          

            $scope.isBtnAddSkBkpmShowed = false;
            $scope.isBtnSaveSkBkpmShowed = true;
            $scope.isBtnCancelSkBkpmShowed = true;

            $scope.isAddSkBkpm = false;
            $scope.isEditSkBkpm = true;
        });


    }



    $scope.cancelTambahEditSkBkpmClick = function () {
        $scope.isAddSkBkpm = false;
        $scope.isEditSkBkpm = false;

        $scope.isBtnAddSkBkpmShowed = true;
        $scope.isBtnSaveSkBkpmShowed = false;
        $scope.isBtnCancelSkBkpmShowed = false;

    }

    $scope.saveTambahEditSkBkpmClick = function () {

        if ($scope.isAddSkBkpm == true) {
           $scope.saveSkBkpm();
        }
        else if ($scope.isEditSkBkpm == true) {

            $scope.saveChangesSkBkpm();
        }


        

        $scope.isAddSkBkpm = false;
        $scope.isEditSkBkpm = false;

        $scope.isBtnAddSkBkpmShowed = true;
        $scope.isBtnSaveSkBkpmShowed = false;
        $scope.isBtnCancelSkBkpmShowed = false;

    }

    $scope.loadSkBkpm = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadSkBkpm/' + $scope.idx)
        .success(function (data) {
            $scope.skBkpm = data.skBkpm;
            $timeout(function () {
                $scope.loadSkBkpmIupOp();
               // $scope.loadSkBkpmTujuanPenjualan();
            }, 3000)

        })
        .finally(function () { })
    }
    $scope.loadSkBkpm();

    $scope.saveSkBkpm = function () {

        $http.post('/angkutjual/bkpms/createservice', {
            ID: '-',
            SkNumber: $scope.skBkpmModel.SkNumber,
            LetterNumber: $scope.skBkpmModel.LetterNumber,
            LetterDate: $('#skbkpmletterdate').val(),
            LetterType: $scope.skBkpmModel.LetterType,
            BKPMAcceptanceDate: $('#BKPMAcceptanceDate').val(),
            EvaluatorAcceptanceDate: $('#EvaluatorAcceptanceDate').val(),
            AdditionalInformation: $scope.skBkpmModel.AdditionalInformation,

            //SkDate: $('#tglskbkpm').val(),
            //SkEndDate: $('#tglberakhirskbkpm').val(),//$scope.skAwalModel.tglberakhir,
            //SkDuration: $scope.skBkpmModel.jangkawaktu,
            //RpiitNumber: $scope.skBkpmModel.RpiitNumber,
            //RpiitDate: $scope.skBkpmModel.RpiitDate,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
        $scope.loadSkBkpm();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK BKPM';
            $scope.modelUpload.moduleType = '9'
            //$('#addSkIupOp').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.skBkpmModel();
        })
    }

    $scope.saveChangesSkBkpm = function () {

        $http.post('/angkutjual/BKPMs/editservice', {
            ID: $scope.skBkpmModel.id,
            LetterNumber: $scope.skBkpmModel.LetterNumber,
            LetterType: $scope.skBkpmModel.LetterType,
            LetterDate: $('#skbkpmletterdate').val(),
            SkNumber: $scope.skBkpmModel.SkNumber,
            BKPMAcceptanceDate: $('#BKPMAcceptanceDate').val(),
            EvaluatorAcceptanceDate: $('#EvaluatorAcceptanceDate').val(),     
            AdditionalInformation: $scope.skBkpmModel.AdditionalInformation,
            Status: $scope.skBkpmModel.Status,
            CompanyID: $scope.idx,
            SKFile: $scope.skBkpmModel.SKFile
        })
        .success(function (data) {
            $scope.loadSkBkpm();
           $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'SK BKPM';
            $scope.modelUpload.moduleType = '9'
            //$('#addSkIupOp').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initShareholderModel();
        })
    }


    $scope.tambahSkBkpmIupOp = function () {
        $scope.initSkBkpmIupOpModel();
        $('#addSkBkpmIupOp').modal('show');
    }

    $scope.simpanSkBkpmIupOp = function () {
        $http.post('/angkutjual/coalsourcesks/createservice', {
            SkNumber: $scope.skBkpmIupModel.skNumber,
            SkDate: $scope.skBkpmIupModel.skDate,
            SkFile: $scope.skBkpmIupModel.skFile,
            CompanySource: $scope.skBkpmIupModel.companySource,
            CompanySourceAddress: $scope.skBkpmIupModel.companySourceAddress,
            City: $scope.skBkpmIupModel.city,
            Province: $scope.skBkpmIupModel.province,
            SalesDestination: $scope.skBkpmIupModel.salesDestination,
            Remark: $scope.skBkpmIupModel.remark,
            Tonnage: $scope.skBkpmIupModel.tonnage,
            BKPMID: $('#skBkpmId').val()

        })
        .success(function (data) {
            $scope.loadSkBkpmIupOp();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'IUP OP SK Bkpm';
            $scope.modelUpload.moduleType = '10'
            $('#addSkBkpmIupOp').modal('hide');
            $scope.uploadFile();


        })
        .finally(function () { })
    }

    $scope.loadSkBkpmIupOp = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadIupOpSkBkpm/' + $('#skBkpmId').val())
        .success(function (data) {
            $scope.coalsourcesksBkpm = data.data;
        })
    }

    $scope.hapusSkBkpmIupOp = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/coalsourcesks/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadSkBkpmIupOp();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //END OF SK  BKPM


    //ET RECOMMENDATION

    $scope.isAddETRecommendation = false;
    $scope.isEditETRecommendation = false;
    $scope.isBtnAddETRecommendationShowed = true;
    $scope.isBtnSaveETRecommendationShowed = false;
    $scope.isBtnCancelETRecommendationShowed = false;

    $scope.tambahETRecommendationClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addETRecommendation').modal('show');

        $scope.isBtnAddETRecommendationShowed = false;
        $scope.isBtnSaveETRecommendationShowed = true;
        $scope.isBtnCancelETRecommendationShowed = true;

        if ($scope.isAddETRecommendation) {
            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddETRecommendation = true;
            $scope.isEditETRecommendation = false;
        }
    }

    $scope.cancelTambahEditETRecommendationClick = function () {
        $scope.isAddETRecommendation = false;
        $scope.isEditETRecommendation = false;

        $scope.isBtnAddETRecommendationShowed = true;
        $scope.isBtnSaveETRecommendationShowed = false;
        $scope.isBtnCancelETRecommendationShowed = false;

    }

    $scope.saveTambahEditETRecommendationClick = function () {

        $scope.saveETRecommendation();

        $scope.isAddETRecommendation = false;
        $scope.isEditETRecommendation = false;

        $scope.isBtnAddETRecommendationShowed = true;
        $scope.isBtnSaveETRecommendationShowed = false;
        $scope.isBtnCancelETRecommendationShowed = false;

    }

    $scope.loadETRecommendation = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadETRecommendation/' + $scope.idx)
        .success(function (data) {
            $scope.ETRecommendation = data.ETRecommendation;

        })
        .finally(function () { })
    }
    $scope.loadETRecommendation();

    $scope.saveETRecommendation = function () {


        $http.post('/angkutjual/ETRecommendations/createservice', {
            ID: '-',
            SkNumber: $scope.ETRecommendationModel.SkNumber,
            SkDate: $('#tglETRecommendation').val(),// $scope.skAwalModel.tglsk,      
            AdditionalInformation: $scope.ETRecommendationModel.SkAdditionalInfo,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadETRecommendation();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'REKOMENDASI ET';
            $scope.modelUpload.moduleType = '11';
            $('#addETRecommendation').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initETRecommendationModel();
        })
    }

    $scope.hapusETRecommendation = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/ETRecommendations/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadETRecommendation();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    //END OF ET RECOMMENDATION



    //ADDITIONAL COOPERATION

    $scope.isAddAdditionalCooperation = false;
    $scope.isEditAdditionalCooperation = false;
    $scope.isBtnAddAdditionalCooperationShowed = true;
    $scope.isBtnSaveAdditionalCooperationShowed = false;
    $scope.isBtnCancelAdditionalCooperationShowed = false;

    //$scope.initAdditionalCooperationModel() = function () {
    //    $scope.AdditionalCooperationModel = {
    //        SkNumber: '',
    //        SkDate: '',
    //        SkAdditionalInfo:''
    //}
    //}
    //$scope.initAdditionalCooperationModel();

    $scope.tambahAdditionalCooperationClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addAdditionalCooperation').modal('show');
        $scope.isBtnAddAdditionalCooperationShowed = false;
        $scope.isBtnSaveAdditionalCooperationShowed = true;
        $scope.isBtnCancelAdditionalCooperationShowed = true;

        if ($scope.isAddAdditionalCooperation) {
            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddAdditionalCooperation = true;
            $scope.isEditAdditionalCooperation = false;
        }
    }

    $scope.cancelTambahEditAdditionalCooperationClick = function () {
        $scope.isAddAdditionalCooperation = false;
        $scope.isEditAdditionalCooperation = false;

        $scope.isBtnAddAdditionalCooperationShowed = true;
        $scope.isBtnSaveAdditionalCooperationShowed = false;
        $scope.isBtnCancelAdditionalCooperationShowed = false;

    }

    $scope.saveTambahEditAdditionalCooperationClick = function () {

        $scope.saveAdditionalCooperation();

        $scope.isAddAdditionalCooperation = false;
        $scope.isEditAdditionalCooperation = false;

        $scope.isBtnAddAdditionalCooperationShowed = true;
        $scope.isBtnSaveAdditionalCooperationShowed = false;
        $scope.isBtnCancelAdditionalCooperationShowed = false;

    }

    $scope.loadAdditionalCooperation = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadAdditionalCooperation/' + $scope.idx)
        .success(function (data) {
            $scope.AdditionalCooperation = data.AdditionalCooperation;

        })
        .finally(function () { })
    }
    $scope.loadAdditionalCooperation();

    $scope.saveAdditionalCooperation = function () {


        $http.post('/angkutjual/AdditionalCooperations/createservice', {
            ID: '-',
            SkNumber: $scope.AdditionalCooperationModel.SkNumber,
            SkDate: $('#tglAdditionalCooperation').val(),// $scope.skAwalModel.tglsk,      
            AdditionalInformation: $scope.AdditionalCooperationModel.SkAdditionalInfo,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadAdditionalCooperation();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = ';PENAMBAHAN KERJASAMA';
            $scope.modelUpload.moduleType = '12';
            $('#addAdditionalCooperation').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initAdditionalCooperationModel();
        })
    }

    $scope.hapusAdditionalCooperation = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/AdditionalCooperations/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadAdditionalCooperation();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    //END OF ADDITIONAL COOPERATION
    


    // CNC CERTIFICATE

    $scope.isAddCNCCertificate = false;
    $scope.isEditCNCCertificate = false;
    $scope.isBtnAddCNCCertificateShowed = true;
    $scope.isBtnSaveCNCCertificateShowed = false;
    $scope.isBtnCancelCNCCertificateShowed = false;

    $scope.tambahCNCCertificateClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addCNCCertificate').modal('show');

        $scope.isBtnAddCNCCertificateShowed = false;
        $scope.isBtnSaveCNCCertificateShowed = true;
        $scope.isBtnCancelCNCCertificateShowed = true;

        if ($scope.isAddCNCCertificate) {
            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddCNCCertificate = true;
            $scope.isEditCNCCertificate = false;
        }
    }

    $scope.cancelTambahEditCNCCertificateClick = function () {
        $scope.isAddCNCCertificate = false;
        $scope.isEditCNCCertificate = false;

        $scope.isBtnAddCNCCertificateShowed = true;
        $scope.isBtnSaveCNCCertificateShowed = false;
        $scope.isBtnCancelCNCCertificateShowed = false;

    }

    $scope.saveTambahEditCNCCertificateClick = function () {

        $scope.saveCNCCertificate();

        $scope.isAddCNCCertificate = false;
        $scope.isEditCNCCertificate = false;

        $scope.isBtnAddCNCCertificateShowed = true;
        $scope.isBtnSaveCNCCertificateShowed = false;
        $scope.isBtnCancelCNCCertificateShowed = false;

    }

    $scope.loadCNCCertificate = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadCNCCertificate/' + $scope.idx)
        .success(function (data) {
            $scope.CNCCertificate = data.CNCCertificate;

        })
        .finally(function () { })
    }
    $scope.loadCNCCertificate();

    $scope.saveCNCCertificate = function () {


        $http.post('/angkutjual/CNCCertificates/createservice', {
            ID: '-',
            SkNumber: $scope.CNCCertificateModel.SkNumber,
            SkDate: $('#tglCNCCertificate').val(),// $scope.skAwalModel.tglsk,      
            AdditionalInformation: $scope.CNCCertificateModel.SkAdditionalInfo,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            $scope.loadCNCCertificate();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'CNC Certificate';
            $scope.modelUpload.moduleType = '13';
            $('#addCNCCertificate').modal('hide');
            $scope.uploadFile();

        })
        .error(function () {

        })
        .finally(function () {
            $scope.initCNCCertificateModel();
        })
    }

    $scope.hapusCNCCertificate = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/CNCCertificates/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadCNCCertificate();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    //END OF CNC CERTIFICATE



    // COMPANY FILES

    $scope.isAddCompanyFile = false;
    $scope.isEditCompanyFile = false;
    $scope.isBtnAddCompanyFileShowed = true;
    $scope.isBtnSaveCompanyFileShowed = false;
    $scope.isBtnCancelCompanyFileShowed = false;

    $scope.tambahCompanyFileClick = function () {

        $scope.initSkPerpanjanganIupOpModel();
        $('#addCompanyFile').modal('show');

        $scope.isBtnAddCompanyFileShowed = false;
        $scope.isBtnSaveCompanyFileShowed = true;
        $scope.isBtnCancelCompanyFileShowed = true;

        if ($scope.isAddCompanyFile) {
            //simpan or edit data
        }
        else {
            //init tambah data
            $scope.isAddCompanyFile = true;
            $scope.isEditCompanyFile = false;
        }
    }

    $scope.cancelTambahEditCompanyFileClick = function () {
        $scope.isAddCompanyFile = false;
        $scope.isEditCompanyFile = false;

        $scope.isBtnAddCompanyFileShowed = true;
        $scope.isBtnSaveCompanyFileShowed = false;
        $scope.isBtnCancelCompanyFileShowed = false;

    }

    $scope.saveTambahEditCompanyFileClick = function () {
        $scope.saveCompanyFile();
        $scope.isAddCompanyFile = false;
        $scope.isEditCompanyFile = false;

        $scope.isBtnAddCompanyFileShowed = true;
        $scope.isBtnSaveCompanyFileShowed = false;
        $scope.isBtnCancelCompanyFileShowed = false;
    }

    $scope.loadCompanyFile = function () {
        $http.get('/angkutjual/iupopangkutjual/LoadCompanyFile/' + $scope.idx)
        .success(function (data) {
            $scope.CompanyFile = data.CompanyFile;
        })
        .finally(function () { })
    }
    $scope.loadCompanyFile();

    $scope.saveCompanyFile = function () {
        $http.post('/Organization/CompanyFiles/createservice', {
            ID: '-',
            NomorSurat: $scope.CompanyFileModel.NomorSurat,
            NamaSurat: $scope.CompanyFileModel.NamaSurat,
            Pengirim: $scope.CompanyFileModel.Pengirim,
            Perihal: $scope.CompanyFileModel.Perihal,
            Tujuan: $scope.CompanyFileModel.Tujuan,
            TanggalSurat: $('#TanggalSurat').data('datepicker').getFormattedDate('mm/dd/yyyy'),
            Description: $scope.CompanyFileModel.Description,
            CompanyID: $scope.idx,
        })
        .success(function (data) {
            debugger;
            $scope.loadCompanyFile();
            $scope.modelUpload.id = data;
            $scope.modelUpload.title = 'Company File';
            $scope.modelUpload.moduleType = '14';
            $('#addCompanyFile').modal('hide');
            $scope.uploadFile();
        })
        .error(function () {

        })
        .finally(function () {
            $scope.initCompanyFileModel();
        })
    }

    $scope.hapusCompanyFile = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
            $http.post('/Organization/CompanyFiles/DeleteService', { id: id })
            .success(function (data) {
                $scope.loadCompanyFile();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    //END OF COMPANY FILES

    //Report
    $scope.tambahReport = function () {
        $scope.initReportModel();
        $scope.ReportModel.Rkab = 0;
        $scope.ReportModel.Q1 = 0;
        $scope.ReportModel.Q2 = 0;
        $scope.ReportModel.Q3 = 0;
        $scope.ReportModel.Q4 = 0;
        $scope.ReportModel.Annual = 0;
        $scope.isEditReport = false;
        $('#addReport').modal('show');
    }
    $scope.simpanReport = function () {
        debugger;
        $http.post('/angkutjual/iupopangkutjual/CreateReportService', {
            ID: '-',
            CompanyID: $scope.idx,
            Rkab: $('#Rkab').val(),
            Q1: $('#Q1').val(),
            Q2: $('#Q2').val(),
            Q3: $('#Q3').val(),
            Q4: $('#Q4').val(),
            Annual: $('#Annual').val(),
            Year: $('#Year').val()
        })
        .success(function (data) {
            debugger;
            $scope.loadReport();
            $('#addReport').modal('hide');
            
        })
        .finally(function () {
            $scope.initReportModel();
        })
    }

    $scope.editReport = function (id) {
        $scope.isEditReport = true;
        $http.get('/angkutjual/iupopangkutjual/findReport/' + id)
        .success(function (data) {
            $scope.ReportModel.ID = data.ID;
            $scope.ReportModel.Rkab = data.Rkab;
            $scope.ReportModel.Q1 = data.Q1;
            $scope.ReportModel.Q2 = data.Q2;
            $scope.ReportModel.Q3 = data.Q3;
            $scope.ReportModel.Q4 = data.Q4;
            $scope.ReportModel.Annual = data.Annual;
            
            $('#Year').val(data.Year)

            $scope.RkabModel.CompanyID = data.CompanyID;

            $('#addReport').modal('show');
        });
    }

    $scope.simpanEditReport = function () {
        $http.post('/angkutjual/iupopangkutjual/EditReportService', {
            ID: $scope.ReportModel.ID,
            CompanyID: $scope.idx,
            Rkab: $('#Rkab').val(),
            Q1: $('#Q1').val(),
            Q2: $('#Q2').val(),
            Q3: $('#Q3').val(),
            Q4: $('#Q4').val(),
            Annual: $('#Annual').val(),
            Year: $('#Year').val()            
        })
        .success(function (data) {
            
            $scope.loadReport();
            $('#addReport').modal('hide');
            $scope.isEditReport = false;
        })
        .finally(function () { })
    }

    $scope.hapusReport = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/iupopangkutjual/DeleteReport', { id: id })
            .success(function (data) {
                $scope.loadReport();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //RKAB
    
    $scope.tambahRkab = function () {
        $scope.initRkabModel();
        $scope.isEditRkab = false;
        $('#addRKAB').modal('show');
    }

    $scope.isEditRkab = false;
    $scope.editRkab = function (id) {

        $scope.isEditRkab = true;
        $http.get('/angkutjual/RKAB/findbyid/' + id)
        .success(function (data) {
            $scope.RkabModel.ID = data.ID;
            $scope.RkabModel.RkabYear = data.RkabYear;
            $('#RkabYear').val(data.RkabYear)
            $scope.RkabModel.Status = data.Status;
            if ($scope.RkabModel.Status == true) {
                $('#RkabStatus').val(1);
            }
            else { $('#RkabStatus').val(0); }          

            $scope.RkabModel.CompanyID = data.CompanyID;
           
            $('#addRKAB').modal('show');
        });        
    }

    $scope.simpanRkab = function () {
        var status = false;
        if ($('#RkabStatus').val() == 1) { status = true; }
        $http.post('/angkutjual/RKAB/createservice', {
            ID : '-',
            CompanyID: $scope.idx,
            RkabYear: $('#RkabYear').val(),
            Status: status ,
        })
        .success(function (data) {
            $scope.loadRkab();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
             $('#addRKAB').modal('hide');
            //$scope.uploadFile();

          
        })
        .finally(function () { })
    }

    $scope.simpanEditRkab = function () {
        var status = false;
        if ($('#RkabStatus').val() == 1) { status = true; }
        $http.post('/angkutjual/RKAB/editservice', {
            ID: $scope.RkabModel.ID,
            CompanyID: $scope.idx,
            RkabYear: $('#RkabYear').val(),
            Status: status,

        })
        .success(function (data) {
            $scope.loadRkab();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
            $('#addRKAB').modal('hide');
            //$scope.uploadFile();
            $scope.isEditRkab = false;

        })
        .finally(function () { })
    }

    $scope.loadRkab = function () {
        $http.get('/angkutjual/iupopangkutjual/loadrkab/' + $scope.idx)
        .success(function (data) {
            $scope.rkab = data.rkab;
        })
    }
    $scope.loadRkab();

    $scope.loadReport = function(){
        $http.get('/angkutjual/iupopangkutjual/LoadReport/' + $scope.idx)
        .success(function (data) {
            $scope.report = data.report;
        })
    }
    $scope.loadReport();

    $scope.hapusRkab = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {
           
            $http.post('/angkutjual/RKAB/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadRkab();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }
    
    $scope.tambahQuarter = function () {
        $scope.initQuarterModel();
        $scope.isEditQuarter = false;
        $('#addQuarter').modal('show');
    }

    $scope.isEditQuarter = false;
    $scope.editQuarter = function (id) {

        $scope.isEditQuarter = true;
        $http.get('/angkutjual/Quarter/findbyid/' + id)
        .success(function (data) {
            $scope.QuarterModel.ID = data.ID;
            $scope.QuarterModel.QuarterYear = data.Year;
            $('#QuarterYear').val(data.Year)
            $scope.QuarterModel.Period = data.Period;
            $('#QuarterPeriod').val(data.Period)

            $scope.QuarterModel.Status = data.Status;
            if ($scope.QuarterModel.Status == true) {
                $('#QuarterStatus').val(1);
            }
            else { $('#QuarterStatus').val(0); }


            $scope.QuarterModel.CompanyID = data.CompanyID;

            $('#addQuarter').modal('show');
        });
    }

    $scope.simpanQuarter = function () {
        var status = false;
        if ($('#QuarterStatus').val() == 1) { status = true; }
        $http.post('/angkutjual/Quarter/createservice', {
            ID: '-',
            CompanyID: $scope.idx,
            Year: $('#QuarterYear').val(),
            Status: status,
            Period: $('#QuarterPeriod').val()


        })
        .success(function (data) {
            $scope.loadQuarter();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
            $('#addQuarter').modal('hide');
            //$scope.uploadFile();


        })
        .finally(function () { })
    }

    $scope.simpanEditQuarter = function () {
        var status = false;
        if ($('#QuarterStatus').val() == 1) { status = true; }
        $http.post('/angkutjual/Quarter/editservice', {
            ID: $scope.QuarterModel.ID,
            CompanyID: $scope.idx,
            Year: $('#QuarterYear').val(),
            Status: status,
            Period: $('#QuarterPeriod').val()

        })
        .success(function (data) {
            $scope.loadQuarter();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
            $('#addQuarter').modal('hide');
            //$scope.uploadFile();
            $scope.isEditQuarter = false;

        })
        .finally(function () { })
    }

    $scope.loadQuarter = function () {
        $http.get('/angkutjual/iupopangkutjual/loadQuarter/' + $scope.idx)
        .success(function (data) {
            $scope.quarter = data.quarter;
        })
    }
    
    $scope.hapusQuarter = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/Quarter/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadQuarter();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //END OF QUARTER   


    //ANNUALY

    $scope.tambahAnnual = function () {
        $scope.initAnnualModel();
        $scope.isEditAnnual = false;
        $('#addAnnual').modal('show');
    }

    $scope.isEditAnnual = false;
    $scope.editAnnual = function (id) {

        $scope.isEditAnnual = true;
        $http.get('/angkutjual/Annual/findbyid/' + id)
        .success(function (data) {
            $scope.AnnualModel.ID = data.ID;
            $scope.AnnualModel.Year = data.Year;
            $('#AnnualYear').val(data.Year)


            $scope.AnnualModel.Status = data.Status;
            if ($scope.AnnualModel.Status == true) {
                $('#AnnualStatus').val(1);
            }
            else { $('#AnnualStatus').val(0); }


            $scope.AnnualModel.CompanyID = data.CompanyID;

            $('#addAnnual').modal('show');
        });
    }

    $scope.simpanAnnual = function () {
        var status = false;
        if ($('#AnnualStatus').val() == 1) { status = true; }
        $http.post('/angkutjual/Annual/createservice', {
            ID: '-',
            CompanyID: $scope.idx,
            Year: $('#AnnualYear').val(),
            Status: status,


        })
        .success(function (data) {
            $scope.loadAnnual();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
            $('#addAnnual').modal('hide');
            //$scope.uploadFile();


        })
        .finally(function () { })
    }

    $scope.simpanEditAnnual = function () {
        var status = false;
        if ($('#AnnualStatus').val() == 1) { status = true; }
        $http.post('/angkutjual/Annual/editservice', {
            ID: $scope.AnnualModel.ID,
            CompanyID: $scope.idx,
            Year: $('#AnnualYear').val(),
            Status: status,

        })
        .success(function (data) {
            $scope.loadAnnual();
            //$scope.modelUpload.id = data;
            //$scope.modelUpload.title = 'IUP OP SK Awal';
            //$scope.modelUpload.moduleType = '2'
            $('#addAnnual').modal('hide');
            //$scope.uploadFile();
            $scope.isEditAnnual = false;

        })
        .finally(function () { })
    }

    $scope.loadAnnual = function () {
        $http.get('/angkutjual/iupopangkutjual/loadAnnual/' + $scope.idx)
        .success(function (data) {
            $scope.annual = data.annual;
        })
    }
    $scope.loadAnnual();
    $scope.hapusAnnual = function (id) {
        swal({
            title: "Hapus data?", text: "Data yang sudah dihapus tidak bisa dikembalikan",
            type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ya, silakan hapus!", closeOnConfirm: false
        }, function () {

            $http.post('/angkutjual/Annual/deleteservice', { id: id })
            .success(function (data) {
                $scope.loadAnnual();
                swal("Sukses!", "Data terhapus", "success");
            })

        });
    }

    //END OF ANNUALY

  
})