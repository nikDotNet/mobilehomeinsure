var CustomerActions = function () {
    var oTable = null;

    var handleTable = function (url, tableColumns) {
        $.ajaxSetup({
            cache: false
        });


        var table = $('#tblLists');
        oTable = table.DataTable({

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
            orderCellsTop: true,
            "bProcessing": true,
            "bServerSide": true,
            "sServerMethod": "POST",
            "sAjaxSource": url,
            "stateSave": true,
            "aoColumns": tableColumns,
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                //debugger;
                //Need some customization, we can write here

                if (typeof rowCallbackHandler === 'function') {
                    rowCallbackHandler(nRow, aData, iDisplayIndex, iDisplayIndexFull);
                }               
            },
            "initComplete": function (settings, json) {
                //debugger;
                if (typeof viewEventInit === 'function') {
                    viewEventInit();
                }
            },
            "fnDrawCallback": function (oSettings) {
                if (typeof viewEventInit === 'function') {
                    viewEventInit();

                }
                
                if (s === "") {
                    oTable.state.clear();
                    window.location.reload();
                }
                var s = window.location.hash.replace("#", "");

                var obj = s === "" ? {} : JSON.parse('{"' + decodeURI(s.replace(/&/g, "\",\"").replace(/=/g, "\":\"")) + '"}');
                
                $("#tblLists thead input").each(function (i, e) {
                    //alert(obj['C' + i]);
                    if ('C' + i in obj && obj['C' + i] !== undefined) {
                        //alert(obj['C' + i]);
                        this.value = obj['C' + i];
                    }
                });
            }
        });

        $("ul.sub-menu li").click(function () {
            var url = window.location.href;
            if ($(this).find("a").prop("href").toLowerCase() === url.toLowerCase()) {
                oTable.state.clear();
                window.location.reload();
            }
        }) 

        // Apply the filter with header textbox
        $("#tblLists thead input").on('keyup change', function () {
            var hash = "";
            var cindex = $(this).parent().index();
            var scount = 0;
            $("#tblLists thead input").each(function (i, e) {

                if (this.value != "" && i != cindex) {
                    if(scount == 0)
                        hash = 'C' + i + "=" + this.value;
                    else
                        hash = '&C' + i + "=" + this.value;
                    scount++;
                }
            });
            if (scount == 0)
                window.location.hash = '#' + hash + 'C' + $(this).parent().index() + '=' + this.value;
            else
                window.location.hash = '#' + hash + '&C' + $(this).parent().index() + '=' + this.value;

            oTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
            oTable.state.save();
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
    };

    return {

        //main function to initiate the module
        init: function (url, tableColumns) {
            handleTable(url, tableColumns);
        }
    };
}();