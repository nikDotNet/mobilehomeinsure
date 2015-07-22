﻿var CustomerActions = function () {

    var handleTable = function (url, tableColumns) {
        $.ajaxSetup({
            cache: false
        });

        //function restoreRow(oTable, nRow) {
        //    debugger;
        //    var aData = oTable.fnGetData(nRow);
        //    var jqTds = $('>td', nRow);

        //    for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
        //        oTable.fnUpdate(aData[i], nRow, i, false);
        //    }

        //    oTable.fnDraw();
        //}

        //function editRow(oTable, nRow) {
        //    debugger;
        //    var aData = oTable.fnGetData(nRow);
        //    var jqTds = $('>td', nRow);
        //    //jqTds[0].innerHTML = '<span id="Id">' + aData[0] + '</span>';
        //    //jqTds[1].innerHTML = '<input type="text" class="form-control input-small" id="txtAge" value="' + aData[1] + '">';
        //    //jqTds[2].innerHTML = '<input type="text" class="form-control input-small" id="txtFactor" value="' + aData[2] + '">';
        //    ////jqTds[3].innerHTML = '<input type="text" class="form-control input-small" id="" value="' + aData[3] + '">';
        //    //jqTds[4].innerHTML = '<a class="edit" href="">Save</a>';
        //    //jqTds[5].innerHTML = '<a class="cancel" href="">Cancel</a>';
        //}

        //function saveRow(oTable, nRow) {
        //    debugger;
        //    var jqInputs = $('input', nRow);
        //    var options = {};
        //    options.url = "saveAgeFactor";
        //    options.method = "POST";
        //    options.data = { Id: $("#Id").text(), Age: $("#txtAge").val(), Factor: $("#txtFactor").val() };
        //    var request = $.ajax(options);

        //    request.done(function (msg) {
        //        alert("Success");
        //    });

        //    request.fail(function (jqXHR, textStatus) {
        //        alert("Request failed: " + textStatus);
        //    });


        //    oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
        //    oTable.fnUpdate(jqInputs[1].value, nRow, 1, false);
        //    oTable.fnUpdate(jqInputs[2].value, nRow, 2, false);
        //    //oTable.fnUpdate(jqInputs[3].value, nRow, 3, false);
        //    oTable.fnUpdate('<a class="edit" href="">Edit</a>', nRow, 4, false);
        //    oTable.fnUpdate('<a class="delete" href="">Delete</a>', nRow, 5, false);
        //    oTable.fnDraw();
        //}

        //function cancelEditRow(oTable, nRow) {
        //    debugger;
        //    var jqInputs = $('input', nRow);
        //    oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
        //    oTable.fnUpdate(jqInputs[1].value, nRow, 1, false);
        //    oTable.fnUpdate(jqInputs[2].value, nRow, 2, false);
        //    //oTable.fnUpdate(jqInputs[3].value, nRow, 3, false);
        //    oTable.fnUpdate('<a class="edit" href="">Edit</a>', nRow, 4, false);
        //    oTable.fnDraw();
        //}


        var table = $('#tblLists');
        var oTable = table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            dom: 'T<"clearfix">lfrtip',
            tableTools: {
                "sSwfPath": "../../Content/assets/global/plugins/TableToolsv2.2.4/swf/copy_csv_xls_pdf.swf"
            },
            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "All"] // change per page values here
            ],

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},

            // set the initial value
            "pageLength": 10,

            "language": {
                "lengthMenu": " _MENU_ records"
            },
            "columnDefs": [{ // set default column settings
                'orderable': true,
                'targets': [0]
            }, {
                "searchable": true,
                "targets": [0]
            }],
            "order": [
                [0, "asc"]
            ], // set first column as a default sort by asc

            //Vikas did generic modification for all table initialisation
            //"bFilter": false, //disabled common search 
            "bProcessing": true,
            "bServerSide": true,
            "sServerMethod": "POST",
            "sAjaxSource": url,
            "aoColumns": tableColumns,
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                //debugger;
                //Need some customization, we can write here

                $('td:eq(7)', nRow).find('a').each(function (index, element) {
                    //debugger;
                    if ($(element).hasClass("delete-link")) {
                        $(element).attr('href', $(element).attr('href') + aData.Id + "&delType=" + $(element).data('type'));
                    }
                });
            }
        });


        //var tt = new $.fn.dataTable.TableTools(oTable);
        //tt.sSwfPath = "../../Content/assets/global/plugins/TableToolsv2.2.4/swf/copy_csv_xls_pdf.swf";

        //$(tt.fnContainer()).insertBefore('div.dataTables_wrapper');

        var tableWrapper = $("#sample_editable_1_wrapper");

        tableWrapper.find(".dataTables_length select").select2({
            showSearchInput: false //hide search box with special css class
        }); // initialize select2 dropdown

        var nEditing = null;
        var nNew = false;

        $('#sample_editable_1_new').click(function (e) {
            e.preventDefault();

            if (nNew && nEditing) {
                if (confirm("Previse row not saved. Do you want to save it ?")) {
                    saveRow(oTable, nEditing); // save
                    $(nEditing).find("td:first").html("Untitled");
                    nEditing = null;
                    nNew = false;

                } else {
                    oTable.fnDeleteRow(nEditing); // cancel
                    nEditing = null;
                    nNew = false;

                    return;
                }
            }

            var aiNew = oTable.fnAddData(['', '', '', '', '', '']);
            var nRow = oTable.fnGetNodes(aiNew[0]);
            editRow(oTable, nRow);
            nEditing = nRow;
            nNew = true;
        });

        //table.on('click', '.delete', function (e) {
        //    debugger;
        //    e.preventDefault();

        //    if (confirm("Are you sure to delete this row ?") == false) {
        //        return;
        //    }

        //    var nRow = $(this).parents('tr')[0];
        //    oTable.fnDeleteRow(nRow);
        //    alert("Deleted!)");
        //});

        //table.on('click', '.cancel', function (e) {
        //    debugger;
        //    e.preventDefault();
        //    if (nNew) {
        //        oTable.fnDeleteRow(nEditing);
        //        nEditing = null;
        //        nNew = false;
        //    } else {
        //        restoreRow(oTable, nEditing);
        //        nEditing = null;
        //    }
        //});

        //table.on('click', '.edit', function (e) {
        //    debugger;
        //    e.preventDefault();

        //    /* Get the row as a parent of the link that was clicked on */
        //    var nRow = $(this).parents('tr')[0];

        //    if (nEditing !== null && nEditing != nRow) {
        //        /* Currently editing - but not this row - restore the old before continuing to edit mode */
        //        restoreRow(oTable, nEditing);
        //        editRow(oTable, nRow);
        //        nEditing = nRow;
        //    } else if (nEditing == nRow && this.innerHTML == "Save") {
        //        /* Editing this row and want to save it */
        //        saveRow(oTable, nEditing);
        //        nEditing = null;
        //        alert("Updated!)");
        //    } else {
        //        /* No edit in progress - let's start one */
        //        editRow(oTable, nRow);
        //        nEditing = nRow;
        //    }
        //});
    }

    return {

        //main function to initiate the module
        init: function (url, tableColumns) {
            handleTable(url, tableColumns);
        }
    };
}();