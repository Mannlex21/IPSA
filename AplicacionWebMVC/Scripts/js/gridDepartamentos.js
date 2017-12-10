var idD, desD, areaD;
var objD = [];
var descripcionDep = document.getElementById("departamento");
var descripcionA = document.getElementById("departamentoA");
var descripcionD = document.getElementById("departamentoD");
$(function () {
    document.getElementById("divD").style.display = "none";
    $('#agregarDepartamento').on('click', function (e) {
        $('#modalDepartamentos').modal('show');
    });
    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            $('#modalDepartamentos').modal('hide');
        }
    });
    var url = $('#departamentosURL').data('request-url');
    $("#gridDep").jqGrid({
        url: url,
        datatype: 'json',
        mtype: 'Get',
        //table header name   
        colNames: ['Id', 'Descripcion','Area','Control'],
        //colModel takes the data from controller and binds to grid  
        colModel: [
            {
                key: true,
                name: 'idDepartamento',
                index: 'idDepartamento',
                editable: false,
                width: 30,
                resizable: true
            }, {
                key: false,
                name: 'descripcion',
                index: 'descripcion',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    desD = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'area',
                index: 'area',
                editable: false,
                resizable: true, width: 70,
                formatter: function (cellvalue, rowObject) {
                    areaD = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'control',
                index: 'control',
                editable: false,
                resizable: false, width: 22,
                formatter: function (cellvalue, rowObject) {
                    var a = {
                        "idDepartamento": rowObject.rowId,
                        "descripcion": desD,
                        "area": areaD
                    };
                    objD.push(a);
                    return "<button id=btnD" + rowObject.rowId + " class='btn btn-success glyphicon glyphicon-plus btnAgregarD' />";
                }
            }],
        viewrecords: true,
        loadonce: false,
        stringResult: true,
        pager: jQuery('#pagerDep'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: '100%',
        width: '100%',
        caption: 'Tabla departamentos',
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
    $("#gridDep").unbind("contextmenu");
    $('.ui-jqgrid-titlebar-close').hide();
    $("#modalDepartamentos").on('shown.bs.modal', function () {
        $('#gridDep').jqGrid('setGridWidth', $(".modalD").width());
        $(".loaderD").fadeOut(500, function () {
            $("#divD").fadeIn(500);
        });
        
    });
    $(window).on('resize.jqGrid', function () {
        $('#gridDep').jqGrid('setGridWidth', $(".modalD").width());
        
    });
    $(document).on('click', '.btnAgregarD', function (e) {
        var id = e.target.id.replace("btnD", "");
        var v = 0;
        for (var i = 0; i < objD.length; i++) {
            if (objD[i]['idDepartamento'] === id) {
                v++;
                descripcionDep.value = objD[i]['descripcion'];
                descripcionA.value = objD[i]['area'];
                descripcionD.value = objD[i]['idDepartamento'];
                if (v === 1) {
                    $('#modalDepartamentos').modal('hide');
                    $.notify("Se agrego material", "success");
                    return; 
                }
            }
        }
    });
    var url = $('#departamentoURL').data('request-url');
    $.ajax({
        type: "GET",
        url: url,
        data: {
            "username": username,
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            for (var obj in r) {
                descripcionDep.value = r[obj].descripcion;
                descripcionA.value = r[obj].area;
                descripcionD.value = r[obj].idDepartamento;
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("No se encontro");
        }
    });
});