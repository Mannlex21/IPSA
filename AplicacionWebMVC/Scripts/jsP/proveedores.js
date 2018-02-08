$(function () {
    var objM = [];
    var idProveedor, razSoc,rfc;
    document.getElementById("div").style.display = "none";
    var proveedor = document.getElementById("proveedor");
    var nombreProv = document.getElementById("idProvNom");
    var rfc = document.getElementById("rfc");
    var url = $('#ProveedorURL').data('request-url');
    $("#grid").jqGrid({
        url: url,
        datatype: 'json',
        mtype: 'Get',
        //table header name   
        colNames: ['Id','RFC','RazSoc','RazSoc2','Control'],
        //colModel takes the data from controller and binds to grid  
        colModel: [
            {
                key: true,
                name: 'consecutivos',
                index: 'consecutivos',
                editable: false,
                width: 30,
                resizable: true,
                align: 'center',
                sorttype: "int"
            }, {
                key: false,
                name: 'RFC',
                index: 'RFC',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    rfc = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'razSoc',
                index: 'razSoc',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'razSoc2',
                index: 'razSoc2',
                editable: false,
                resizable: true,
                width: 200,
                formatter: function (cellvalue, rowObject) {
                    razSoc = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'control',
                index: 'control',
                editable: false,
                resizable: false, width: 30,
                formatter: function (cellvalue, rowObject) {
                    var a = {
                        "proveedor": rowObject.rowId,
                        "razSoc": razSoc,
                        "RFC":rfc
                    };
                    objM.push(a);
                    return "<button id=btnP" + rowObject.rowId + " class='btn btn-success glyphicon glyphicon-plus btnAgregarP' />";
                }
            }],
        viewrecords: true,
        loadonce: false,
        stringResult: true,
        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: '100%',
        width: '100%',
        caption: 'Tabla materiales',
        loadtext: "Cargando...",
        emptyrecords: 'No se encontraron datos',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        fixed: true,
        autowidth: true,
        multiselect: false,
        shrinkToFit: true,
        beforeSelectRow: function () { return false; }

    });
    $("#grid").unbind("contextmenu");
    $('.ui-jqgrid-titlebar-close').hide();

    $("#modalProveedores").on('shown.bs.modal', function () {
        $('#grid').jqGrid('setGridWidth', $(".modalP").width());
        $(".loaderP").fadeOut(500, function () {
            $("#div").fadeIn(500);
        });
    });
    $("#idProvNom").on('click', function (e) {
        $('#modalProveedores').modal('show');
    });
    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            $('#modalProveedores').modal('hide');
        }
    });
    $(window).on('resize.jqGrid', function () {
        $('#grid').jqGrid('setGridWidth', $(".modalP").width());
    });
    $('#filterButton').click(function (event) {
        var $th = $(this);
        if ($th.hasClass('processing'))
            return;
        $th.addClass('processing');

        filterGrid();
        setTimeout(function () {
            filterGrid();
        }, 500);
        setTimeout(function () {
            filterGrid();
            $th.removeClass('processing');
        }, 1000);

    });
    $('#refreshButton').click(function (event) {
        document.getElementById("rfc").value = "";
        document.getElementById("idProveedor").value = "";
        document.getElementById("razsoc").value = "";
        var $th = $(this);
        if ($th.hasClass('processing'))
            return;
        $th.addClass('processing');

        filterGrid();
        setTimeout(function () {
            filterGrid();
        }, 500);
        setTimeout(function () {
            filterGrid();
            $th.removeClass('processing');
        }, 1000);
    });
    $(document).on('click', '.btnAgregarP', function (e) {
        var id = e.target.id.replace("btnP", "");
        for (var i = 0; i < objM.length; i++) {
            if (objM[i]['proveedor'] === id) {
                proveedor.value = objM[i]['proveedor'];
                nombreProv.value = objM[i]['razSoc'];
                rfc.value = objM[i]['RFC'];
                $('#modalProveedores').modal('hide');
                $.notify("Se agrego material", "success");
                return;
            }
        }
    });
});
function filterGrid() {
    var postDataValues = $("#grid").jqGrid('getGridParam', 'postData');
    $('.filterItem').each(function (index, item) {
        postDataValues[$(item).attr('id')] = $(item).val();
        $("#grid").jqGrid().setGridParam({ postData: postDataValues, page: 1 }).trigger('reloadGrid');

    });
}