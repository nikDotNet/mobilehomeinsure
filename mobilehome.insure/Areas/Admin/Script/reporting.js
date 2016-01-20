var ReportingActions = function () {
    var oTable = null;

    var handleTable = function (url, tableColumns) {
        $.ajaxSetup({
            cache: false
        });


        //table settings
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
                [0, "desc"]
            ], // set first column as a default sort by desc

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

                $('td:eq(8)', nRow).find('a').each(function (index, element) {
                    //debugger;
                    //resolving date problem
                    if ($(element).data("type") == "quote") {
                        var effectiveDate = new Date(parseInt(aData.EffectiveDate.replace("/Date(", "").replace(")/", ""), 10));
                        $('td:eq(5)', nRow).html(effectiveDate.getMonth() + 1 + "/" + effectiveDate.getDate() + "/" + effectiveDate.getFullYear());
                    }

                    if ($(element).hasClass("delete-link")) {
                        $(element).attr('href', $(element).attr('href') + aData.Id + "&delType=" + $(element).data('type'));
                    }
                });
            }
        });


        // Apply the filter with header textbox
        $("#tblLists thead input").on('keyup change', function () {
            //debugger;
            oTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });



        //var tableWrapper = $("#sample_editable_1_wrapper");
        //tableWrapper.find(".dataTables_length select").select2({
        //    showSearchInput: false //hide search box with special css class
        //}); // initialize select2 dropdown

        //var nEditing = null;
        //var nNew = false;

        //$('#sample_editable_1_new').click(function (e) {
        //    e.preventDefault();

        //    if (nNew && nEditing) {
        //        if (confirm("Previse row not saved. Do you want to save it ?")) {
        //            saveRow(oTable, nEditing); // save
        //            $(nEditing).find("td:first").html("Untitled");
        //            nEditing = null;
        //            nNew = false;

        //        } else {
        //            oTable.fnDeleteRow(nEditing); // cancel
        //            nEditing = null;
        //            nNew = false;

        //            return;
        //        }
        //    }

        //    var aiNew = oTable.fnAddData(['', '', '', '', '', '']);
        //    var nRow = oTable.fnGetNodes(aiNew[0]);
        //    editRow(oTable, nRow);
        //    nEditing = nRow;
        //    nNew = true;
        //});
    };

    var search = function () {
        //redraw table with custom filter
        //debugger;
        //oTable.draw();

        if (typeof customSearch === 'function') {
            customSearch(oTable);
        }
    };

    return {

        //main function to initiate the module
        init: function (url, tableColumns) {
            handleTable(url, tableColumns);
        },
        search: search
    };
}();