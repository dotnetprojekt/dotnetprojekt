﻿$(document).ready(function () {

    DatePicker();
    SearchPartners();
    SearchInvoices();
    AddRow();
    //DeleteRow();
    AddInvoiceSearchVendors();
    AddInvoiceSearchBuyers();
    ChangedDiscount();
    //ValidateInvNumber();
    ValidateVatin();
    ValidateVendorVatin();
    ValidateBuyerVatin();
    ValidateMinValue();
    ValidateMaxValue();
    //CountPrice();
    ChangeCommasToDot();

});

$('#invoiceSearchResults').on('click', '#resultTable tbody tr', function () {

    var invoiceId = $(this).attr('title');
    if (invoiceId == "title")
        return;
    var url = "/Home/InvoiceDetails";
    $.post(url, { invoiceId: invoiceId }, function (result) {
        $(".body-content").replaceWith(result)
    });
    
});

$('#goodsTable').on('click', 'tbody tr .delete-button', function () {

    var name = $(this).attr('name');
    //alert(name);
    var number = name.substring(7);
    //alert("aa" + number);
    var rowId = "row" + number;

    $('#goods_' + number + '__name').prop('required', false);
    $('#goods_' + number + '__amount').prop('required', false);
    $('#goods_' + number + '__price').prop('required', false);
    $('#goods_' + number + '__value').prop('required', false);
    $('#goods_' + number + '__tax').prop('required', false);
    $('#goods_' + number + '__gross').prop('required', false);

    $('#goods_' + number + '__name').val("");
    $('#goods_' + number + '__amount').val("");
    $('#goods_' + number + '__price').val("");
    $('#goods_' + number + '__value').val("");
    $('#goods_' + number + '__tax').val("");
    $('#goods_' + number + '__gross').val("");

    /*
    $('#goods_' + number + '__name').removeAttr('required');​​​​​
    $('#goods_' + number + '__amount').removeAttr('required');​​​​​
    $('#goods_' + number + '__price').removeAttr('required');​​​​​
    $('#goods_' + number + '__value').removeAttr('required');​​​​​
    $('#goods_' + number + '__tax').removeAttr('required');​​​​​
    */
    


   // $(this).find('th').each(function (th) {
        //$('#goods_' + rowId + '__name').val("");
   // });

    $('#' + rowId).hide();
});


//$(".delete-button").click(function () {
    //alert("adadsa");
    //var name = $(this).attr('name');
    //alert(name);

//})


function DatePicker() {

    $('.my-date').datepicker({
        format: "dd/mm/yyyy",
        calendarWeeks: true,
        todayHighlight: true,
        placement: 'auto bottom',
        autoclose: true
    });
}

function SearchInvoices() {

    var url = "/Home/SearchInvoice";
    $('#searchForInvoicesButton').click(function (e) {
        e.preventDefault();

        var invNumber = $('#invNumber').val();
        var title = $('#title').val();

        var start = $('#start').val();
        var end = $('#end').val();

        var vname = $('#vname').val();
        var vlastname = $('#vlastname').val();
        var vcompany = $('#vcompany').val();
        var vvatin = $('#vvatin').val();

        var bname = $('#bname').val();
        var blastname = $('#blastname').val();
        var bcompany = $('#bcompany').val();
        var bvatin = $('#bvatin').val();

        var minValue = $('#minValue').val();
        var maxValue = $('#maxValue').val();

        var minValueCorrect = minValue.toString().replace(/\,/g, '.');
        var maxValueCorrect = maxValue.toString().replace(/\,/g, '.');

        $('#invoiceSearchResults').load(url, {
            invNumber: invNumber, start: start, end: end, vname: vname, vlastname: vlastname, title: title,
            vcompany: vcompany, vvatin: vvatin, bname: bname, blastname: blastname, bcompany: bcompany, bvatin: bvatin,
            minValue: minValueCorrect, maxValue: maxValueCorrect
        });

    })
}

/*
function ValidateInvNumber() {

    $('#invNumber').focusout(function () {

        var invNumber = $('#invNumber').val();
        if (!(/^\d+$/.test(invNumber)))
            $('#invNumber').val("");

    })
}
*/

function ValidateVatin() {

    $('#vatin').focusout(function () {

        var vatin = $('#vatin').val();
        if (!(/^\d+$/.test(vatin)))
            $('#vatin').val("");

    })
}

function ValidateVendorVatin() {

    $('#vvatin').focusout(function () {

        var vatin = $('#vvatin').val();
        if (!(/^\d+$/.test(vatin)))
            $('#vvatin').val("");

    })
}

function ValidateBuyerVatin() {

    $('#bvatin').focusout(function () {

        var vatin = $('#bvatin').val();
        if (!(/^\d+$/.test(vatin)))
            $('#bvatin').val("");

    })
}

function ValidateMinValue() {

    $('#minValue').focusout(function () {

        var float = /^\s*(\+|-)?((\d+(\.\d+)?)|(\.\d+))\s*$/;
        var vatin = $('#minValue').val();
        if (!(float.test(vatin)))
            $('#minValue').val("");

    })
}

function ValidateMaxValue() {

    $('#maxValue').focusout(function () {

        var float = /^\s*(\+|-)?((\d+(\.\d+)?)|(\.\d+))\s*$/;
        var vatin = $('#maxValue').val();
        if (!(float.test(vatin)))
            $('#maxValue').val("");

    })
}



function SearchPartners() {

    var url = "/Home/SearchPartnerResults";
    $('#searchForPartnersButton').click(function () {
        var firstName = $('#firstName').val();
        var lastName = $('#lastName').val();
        var companyName = $('#companyName').val();
        var vatin = $('#vatin').val();
        var address = $('#address').val();
        $('#partnerSearchResults').load(url, { firstName: firstName, lastName: lastName, companyName: companyName, vatin: vatin, address: address });
    })
}

function AddRow() {

    $('#addRow').click(function (e) {
        e.preventDefault();
        var count = $('#rowCount').val();
        count++;
        $('#rowCount').val(count);

        var tableRef = document.getElementById('goodsTable').getElementsByTagName('tbody')[0];
        tableRef.insertAdjacentHTML('beforeend', '<tr id="row' + count + '" name="row' + count + '"></tr>');
        var rowName = "row" + count;

        var url = "/Home/AddRow";
        $.post(url, { count: count }, function (result) {
            var changedRow = document.getElementById(rowName);
            changedRow.innerHTML = result;
        });
        
    })
}

/*
function DeleteRow() {

    $(".delete-button").click(function () {
        alert("adadsa");
        //var name = $(this).attr('name');
        //alert(name);

    })
}
*/

function AddInvoiceSearchVendors() {
    
    var url = "/Home/AddInvoiceSearchVendors";
    $('#getVendors').click(function (e) {
        e.preventDefault();
        var vfirstname = $('#vfirstname').val();
        var vlastname = $('#vlastname').val();
        var vcompany = $('#vcompany').val();
        var vvatin = $('#vvatin').val();
        $('#vendorTable tbody').load(url, { firstName: vfirstname, lastName: vlastname, companyName: vcompany, vatin: vvatin });

    })
}


$('#vendorTableDiv').on('click', '#vendorTable tbody tr', function () {

    var rowId = this.id.substring(5);

    var fname = $('#vfirstname_' + rowId).html();
    var lname = $('#vlastname_' + rowId).html();
    var cname = $('#vcompanyname_' + rowId).html();
    var vname = $('#vvatin_' + rowId).html();

    $('#vfirstname').val(fname);
    $('#vlastname').val(lname);
    $('#vcompany').val(cname);
    $('#vvatin').val(vname);

    if (!$(this).hasClass("top-row"))
    {
        $('#vendorTable tr').removeClass('highlighted');
        $(this).addClass('highlighted');
    }

});

function AddInvoiceSearchBuyers() {

    var url = "/Home/AddInvoiceSearchBuyers";
    $('#getBuyers').click(function (e) {
        e.preventDefault();
        var bfirstname = $('#bfirstname').val();
        var blastname = $('#blastname').val();
        var bcompany = $('#bcompany').val();
        var bvatin = $('#bvatin').val();
        $('#buyerTable tbody').load(url, { firstName: bfirstname, lastName: blastname, companyName: bcompany, vatin: bvatin });

    })
}


$('#buyerTableDiv').on('click', '#buyerTable tbody tr', function () {

    var rowId = this.id.substring(5);

    var fname = $('#bfirstname_' + rowId).html();
    var lname = $('#blastname_' + rowId).html();
    var cname = $('#bcompanyname_' + rowId).html();
    var vname = $('#bvatin_' + rowId).html();

    $('#bfirstname').val(fname);
    $('#blastname').val(lname);
    $('#bcompany').val(cname);
    $('#bvatin').val(vname);

    if (!$(this).hasClass("top-row")) {
        $('#buyerTable tr').removeClass('highlighted');
        $(this).addClass('highlighted');
    }

});

$('#goodsTable').on('focusout', 'tbody tr th .goods-data', function () {
    var rowCount = $('#rowCount').val();
    //alert(rowCount);

    var i = 0;
    var netto = 0;
    var brutto = 0;
    
    
    while (i <= rowCount) {
        var name = $('#goods_' + i + '__name').val();
        var amount = $('#goods_' + i + '__amount').val();
        var price = $('#goods_' + i + '__price').val();
        var tax = $('#goods_' + i + '__tax').val();
        //var value = $('#goods_' + i + '__value').val();
        //var gross = $('#goods_' + i + '__gross').val();

        //var value = 0;
        //var localBrutto = 0;

        if (name.length > 0 && amount.length > 0 && price.length > 0 && tax.length > 0)
        {
            var gross = parseFloat(amount) * parseFloat(price);
            $('#goods_' + i + '__gross').val(gross);

            var value = parseFloat(gross) * (1 - parseFloat(tax));
            $('#goods_' + i + '__value').val(value);

            //var varvalue = parseInt(amount) * parseFloat(price);
            //$('#goods_' + i + '__value').val(value);

            //var localBrutto = parseFloat(value) + parseFloat(value) * parseFloat(tax);
            //$('#goods_' + i + '__gross').val(localBrutto);

            netto = netto + parseFloat(value);
            brutto = brutto + parseFloat(gross);
        }        
        i++;
    }

    $('#netto').val(netto.toFixed(2));
    $('#brutto').val(brutto.toFixed(2));

    var discount = $('#discount').val();

    // change comma do dot
    var discountCorrect = discount.toString().replace(/\,/g, '.');
    alert(discountCorrect);
    alert(brutto);

    var totalValue = brutto - parseFloat(discountCorrect) * parseFloat(brutto);

    // change dot to comma
    var correctedTotalValue = totalValue.toString().replace(/\./g, ',');

    $('#value').val(Number(correctedTotalValue).toFixed(2));

});


function ChangedDiscount() {

    $('#discount').focusout(function () {

        var rowCount = $('#rowCount').val();
        //alert(rowCount);

        var i = 0;
        var netto = 0;
        var brutto = 0;

        while (i <= rowCount) {
            var name = $('#goods_' + i + '__name').val();
            var amount = $('#goods_' + i + '__amount').val();
            var price = $('#goods_' + i + '__price').val();
            var tax = $('#goods_' + i + '__tax').val();
            var value = $('#goods_' + i + '__value').val();
            var gross = $('#goods_' + i + '__gross').val();

            if (name.length > 0 && amount.length > 0 && price.length > 0 && value.length > 0 && tax.length > 0 && gross.length > 0) {
                netto = netto + parseFloat(value);
                brutto = brutto + parseFloat(gross);
            }
            i++;
        }

        $('#netto').val(netto.toFixed(2));
        $('#brutto').val(brutto.toFixed(2));
        var discount = $(this).val();

        var bruttoCorrect = brutto.toString().replace(/\,/g, '.');
        var discountCorrect = discount.toString().replace(/\,/g, '.');

        var totalValue = bruttoCorrect - parseFloat(discountCorrect) * parseFloat(bruttoCorrect);

        var correctedTotalValue = totalValue.toString().replace(/\./g, ',');
        $('#value').val(Number(correctedTotalValue).toFixed(2));


    })

}


function ChangeCommasToDot() {

    $('#addButton').click(function () {
        $('.goods-data').each(function (index) {
            //alert( $(this).val() );
            //$(this).val( $(this).val().toString().replace(/\,/g, '.') );
        });
    })
}