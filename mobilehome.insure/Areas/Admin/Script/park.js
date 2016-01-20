var ParkActions = function () {

    var handleTable = function (url, tableColumns) {
        $.ajaxSetup({
            cache: false
        });

        var currentState;
        var currentElement;
        var table = $('#tblLists');
        var oTable = table.DataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            dom: 'T<"clearfix">lfrtip',
            tableTools: {
                "sSwfPath": "../../Content/assets/global/plugins/TableToolsv2.2.4/swf/copy_csv_xls_pdf.swf"
                //"sSwfPath": "http://cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf"
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

            orderCellsTop: true,
            "bProcessing": true,
            "bServerSide": true,
            "sServerMethod": "POST",
            "sAjaxSource": url,
            "aoColumns": tableColumns,
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                //debugger;
                //Need some customization, we can write here

                //appending checkbox for on and off
                $('td:eq(7)', nRow).append(function (index, html) {
                    //debugger;
                    var inputCheckbox = $('<input />',
                                            {
                                                type: 'checkbox',
                                                id: 'cb' + aData.Id,
                                                checked: aData.IsOn
                                            }).data('id', aData.Id).addClass('make-switch');
                    inputCheckbox.data('size', 'small')
                                 .data('on-text', "<i class='fa fa-check'></i>")
                                 .data('off-text', "<i class='fa fa-times'></i>");

                    $(this).html('').append(inputCheckbox);
                });

                $('td:eq(8)', nRow).find('a').each(function (index, element) {
                    //debugger;
                    if ($(element).hasClass("edit-link")) {
                        $(element).attr('href', $(element).attr('href') + aData.Id);
                    }
                    else if ($(element).hasClass("delete-link")) {
                        $(element).attr('href', $(element).attr('href') + aData.Id);
                    }
                });//.html(birthday.getMonth() + 1 + "/" + birthday.getDate() + "/" + birthday.getFullYear());
            },
            "fnDrawCallback": function (oSettings) {
                //debugger;
                resetBootSwitch();
            }
        });

        //reset Switch control
        function resetBootSwitch() {
            //initialise checkbox to bootstrap-switch plugin
            $("input:checkbox").bootstrapSwitch({
                "onSwitchChange": function (event, state) {
                    currentState = state;
                    msg = !currentState ? "Are you sure you want to disable selected Park?" : "Are you sure you want to enable selected Park?"
                    currentElement = $(this);
                    bootbox.confirm({
                        size: 'small',
                        message: msg,
                        callback: function (result) {
                            //debugger;
                            if (result) {
                                //alert('updating value');
                               // debugger;
                                var url = "/admin/park/OnOffPark?id=" + $(currentElement).data('id') + "&isOnOrOff=" + currentState;
                                location.href = url;
                            }
                            else {
                                if (currentElement !== 'undefined' && currentState !== 'undefined') {
                                    //debugger;
                                    $("input:checkbox").bootstrapSwitch('destroy');
                                    currentElement[0].checked = !currentState;
                                    resetBootSwitch();
                                }
                            }
                        }
                    });
                }
            });
        }

        // Apply the filter with header textbox
        $("#tblLists thead input").on('keyup change', function () {
            //debugger;
            oTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
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
                if (confirm("Previous row not saved. Do you want to save it ?")) {
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