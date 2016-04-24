$(document).ready(function () {

    DatePicker();
    SearchPartners();
    SearchInvoices();
    AddRow();
    DeleteRow();

});

$('#invoiceSearchResults').on('click', '#resultTable tbody tr', function () {

    var invoiceNumber = $(this).attr('title');
    var url = "/Home/InvoiceDetails";
    $.post(url, { invoiceNumber: invoiceNumber }, function (result) {
        $(".body-content").replaceWith(result)
    });
    
});

$('#goodsTable').on('click', 'tbody tr .delete-button', function () {

    var name = $(this).attr('name');
    //alert(name);
    var number = name.substring(7);
    //alert(number);
    var rowId = "row" + number;
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
    $('#searchForInvoicesButton').click(function () {
        var invNumber = $('#invNumber').val();
        var vendor = $('#vendor').val();
        var buyer = $('#buyer').val();
        var start = $('#start').val();
        var end = $('#end').val();
        $('#invoiceSearchResults').load(url, { invNumber: invNumber, vendor: vendor, buyer: buyer, start: start, end: end });

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
        //alert(count);
        //var d1 = document.getElementById('goodsTable');
        //var d1 = document.getElementsByClassName('tbody');
        //d1.insertAdjacentHTML('beforeend', '<div id="two">two</div>');

        var tableRef = document.getElementById('goodsTable').getElementsByTagName('tbody')[0];
        tableRef.insertAdjacentHTML('beforeend', '<tr id="row' + count + '" name="row' + count + '"></tr>');
        var rowName = "row" + count;

        var url = "/Home/AddRow";
        $.post(url, { count: count }, function (result) {
            var changedRow = document.getElementById(rowName);
            changedRow.innerHTML = result;
            //$('#row1').replaceWith(result);
            //$("body").replaceWith(result);
            //$(".body-content").replaceWith(result)
        });

        //alert(count);
        
    })
}

/*
function AllowTableRowsToBeClicked() {
    $('#resultTable tbody tr').on('click', function () {
        alert("asdas");

        var invoiceNumber = $(this).attr('title');
        var url = "/Home/InvoiceDetails";
        $.post(url, { invoiceNumber: invoiceNumber }, function (result) {
            $(".body-content").replaceWith(result)
        });
    });
}
*/

function DeleteRow() {

    $(".delete-button").click(function () {
        alert("adadsa");
        //var name = $(this).attr('name');
        //alert(name);

    })
}