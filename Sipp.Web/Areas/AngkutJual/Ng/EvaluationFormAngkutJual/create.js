//var globalInit = 1;
//function addShareholder() {
//    var count = parseInt(globalInit);
//    //$('#rowAdd').append(newElement(count));
//    $("#tableId").find('tbody')
//    .append(
//            newElement(count)
//            );
//    globalInit = count + 1;
//}

//function newElement(value) {
//    return '<tr id="temprow">'+
//                  ' <td class="respons" style="width:249px;!important">' +
//                  ' <input type="text" class="form-control form-filter input-sm ng-pristine ng-untouched ng-valid ng-empty" name="nama" id="shnama'+globalInit+'">' +
//    ' </td>' +
//                  ' <td class="respons" style="width:249px;!important">' +
//                  ' <input type="text" class="form-control form-filter input-sm ng-pristine ng-untouched ng-valid ng-empty" name="jumlah" id="shjumlah '+globalInit+'" >' +
//    '</td>' +
//                  ' <td class="respons" style="width:249px;!important">' +
//                  ' <input type="text" class="form-control form-filter input-sm ng-pristine ng-untouched ng-valid ng-empty" name="nilai" id="shnilai '+globalInit+'">' +
//                  ' </td>' +
//                  ' <td class="respons" style="width:249px;!important">' +
//                  ' <input type="text" class="form-control form-filter input-sm ng-pristine ng-untouched ng-valid ng-empty" name="status" id="shstatus'+globalInit+'">' +
//                  ' </td>' +
//                  ' </tr>'
//}

var app = angular.module('evaluationformangkutjual', []);

app.filter("mydate", function () {
    return function (x) {
        return new Date(parseInt(x.substr(6)));
    };
});

app.controller('evaluationformangkutjual-controller', function ($scope, $http, $interval, $timeout) {
    
    $scope.isAddShareholder = false;

    $scope.initCompanyModel = function () {
        $scope.companyModel = {
            id: '',
            address: '',
            telnumber: '',
            email: '',
            sknumber: '',
            skdate: '',
            sourcecoalsk: '',
            organizer: '',
            //Detail Pelaporan
            reporttype: '',
            reportletternumber: '',
            reportcollectiondate: '',
            dispositionnumber: '',
            //Detail Kegiatan
            sourcecoalreport: '',
            enduserreport: '',
            coaltonase: '',
            activityplan: '',
            activityrealization: '',
            //Detail Keuangan
            currencytype: '',
            revenue: '',
            costofgoodssold: '',
            profitbeforetax: '',
            pph21: '',
            corporatetax: '',
            profit: '',
            evaluatorsnote: ''
        }
    };
    $scope.initCompanyModel();

    $scope.addShareholder = function(){
        $scope.isAddShareholder = true;
    }

    $scope.initShareholderModel = function () {
        $scope.shareholderModel = {
            id: '',
            name: '',
            jumlah: '',
            nilai: '',
            statuswnbh: '',
            isActive: ''
        }
    }
    //$scope.initShareholderModel();

    $scope.onCompanySelectedChange = function (){
        $scope.listCompanyAddress();
    }

    $scope.listCompanyAddress = function () {
        $http.get('/angkutjual/evaluationformangkutjual/CompanyDetails?CompanyId=' + $scope.companyModel.id)
        .success(function (data) {
            debugger;
            $scope.companyModel = {
                id: $scope.companyModel.id,
                address: data.data.Address,
                telnumber: data.data.TelNumber,
                email: data.data.Email,
                sknumber: data.data.SkNumber,
                skdate: $filter('date')(data.data.SkDate2, 'MM/dd/yyyy'),
                sourcecoalsk: data.data.CompanySourceAddress + "/" + data.data.Province
            }
        })
        .error(function () {
            //$scope.initCompanyModel();
        })
    };
  
    $scope.saveEvaluation = function () {
        debugger;
        $http.post('/AngkutJual/EvaluationFormAngkutJual/Create', {
            ID: '-',
            CompanyID: $scope.companyModel.id,
            Address: $scope.companyModel.address,
            TelNumber: $scope.companyModel.telnumber,
            Email: $scope.companyModel.email,
            SkNumber: $scope.companyModel.sknumber,
            SKDate: $scope.companyModel.skdate,
            SourceCoalSk: $scope.companyModel.sourcecoalsk,
            Organizer: $scope.companyModel.organizer
            , NoIntroductoryLetter: $scope.companyModel.reportletternumber
            , NoDisposition: $scope.companyModel.dispositionnumber
            , ReportSubmittedDate: $scope.companyModel.reportcollectiondate
            , CoalOrigin: $scope.companyModel.sourcecoalreport
            , EndUser: $scope.companyModel.enduserreport
            , Tonnage: $scope.companyModel.coaltonase
            , ActivityPlan: $scope.companyModel.activityplan
            , ActivityRealization: $scope.companyModel.activityrealization
            , Revenue: $scope.companyModel.revenue
            , BasicPrice: $scope.companyModel.costofgoodssold
            , ProfitBefore: $scope.companyModel.profitbeforetax
            , OrganizationTax: $scope.companyModel.corporatetax
            , Pph: $scope.companyModel.pph21
            , Profit: $scope.companyModel.profit
            //,RevenueUSD
            //,BasicPriceUSD
            //,ProfitBeforeUSD
            //,OrganizationTaxUSD
            //,PphUSD
            //,ProfitUSD
        })
        .success(function (data) {
            alert('success')
            //$location.path('/angkutjual/evaluationformangkutjual/index')
        })
        .error(function () { })
        .finally(function () { })
    }
    
    $scope.saveShareholder = function () {
        if ($scope.shareholderModel.isActive == '1') { $scope.shareholderModel.isActive = true }
        else {$scope.shareholderModel.isActive = false }
        $http.post('/angkutjual/evaluationformangkutjual/createservice', {
            ID: '-',
            Name: $scope.shareholderModel.name,
            CompanyID:$scope.idx,
            TotalStock: $scope.shareholderModel.jumlah,
            StatusWnBh: $scope.shareholderModel.statuswnbh,
            Status: $scope.shareholderModel.isActive ,
        })
        .success(function (data) {
            //$scope.loadShareholders();
            //$scope.initShareholderModel();
        })
        .error(function () {
            //$scope.initShareholderModel();
        })
    }
})

//function addShareholder() {
//    $(".table table-bordered table-striped table-condensed flip-content").append(
//        '<table class="table table-bordered table-striped table-condensed flip-content">'+
//                                                    '<thead class="flip-content">'+
//                                                    '    <tr>'+
//                                                    '        <th class="respons" style="width:249px;!important">Nama</th>'+
//                                                    '        <th class="respons" style="width:249px;!important">Jumlah</th>'+
//                                                    '        <th class="respons" style="width:249px;!important">Nilai</th>'+
//                                                    '        <th class="respons" style="width:249px;!important">Status WN/BH</th>'+
//                                                    '    </tr>'+
//                                                    '</thead>'+
//                                                    '<tbody>'+
//                                                    '    <tr>'+
//                                                    '        <td class="respons" style="width:249px;!important">'+
//                                                    '            <input type="text" class="form-control form-filter input-sm" name="nama" ng-model="shareholderModel.name">'+
//                                                    '        </td>'+
//                                                    '        <td class="respons" style="width:249px;!important">'+
//                                                    '            <input type="text" class="form-control form-filter input-sm" name="jumlah" ng-model="shareholderModel.jumlah">'+
//                                                    '        </td>'+
//                                                    '        <td class="respons" style="width:249px;!important">'+
//                                                    '            <input type="text" class="form-control form-filter input-sm" name="nilai" ng-model="shareholderModel.nilai">'+
//                                                    '        </td>'+
//                                                    '        <td class="respons" style="width:249px;!important">'+
//                                                    '            <input type="text" class="form-control form-filter input-sm" name="status" ng-model="shareholderModel.statuswnbh">'+
//                                                    '        </td>'+
//                                                    '    </tr>'+
//                                                    '</tbody>'+
//                                                '</table>'

//    )
//}

