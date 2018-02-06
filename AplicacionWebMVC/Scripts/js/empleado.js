var objEmp = [];
var inputEmpleado = document.getElementById('idEmpleado');
$(function () {
    document.getElementById("divE").style.display = "none";
    $('#idEmpleado').on('click', function (e) {
        $('#modalEmpleados').modal('show');
        console.log("entro")
    });
    
    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            $('#modalEmpleados').modal('hide');
        }
    });
    var url = $('#empleadosURL').data('request-url');
    $("#gridEmp").jqGrid({
        url: url,
        datatype: 'json',
        mtype: 'POST',
        //table header name   
        colNames: ['Id', 'Nombre','Apellido paterno','Apellido materno','RFC','Control'],
        //colModel takes the data from controller and binds to grid  
        colModel: [
            {
                key: true,
                name: 'empleado',
                index: 'empleado',
                editable: false,
                align: 'center',
                width: 40,
                resizable: true
            }, {
                key: false,
                name: 'nombre',
                index: 'nombre',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    desD = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'apellidoPaterno',
                index: 'apellidoPaterno',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    desD = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'apellidoMaterno',
                index: 'apellidoMaterno',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    desD = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'rfc',
                index: 'rfc',
                editable: false,
                width: 200,
                resizable: true, formatter: function (cellvalue, rowObject) {
                    desD = cellvalue;
                    return cellvalue;
                }
            }, {
                key: false,
                name: 'control',
                index: 'control',
                editable: false,
                resizable: false,
                width: 45,
                align: 'center',
                formatter: function (cellvalue, rowObject) {
                    var a = {
                        "empleado": rowObject.rowId
                    };
                    objEmp.push(a);
                    return "<button id=btnE" + rowObject.rowId + " class='btn btn-success glyphicon glyphicon-plus btnAgregarE' />";
                }
            }],
        viewrecords: true,
        loadonce: false,
        stringResult: true,
        pager: jQuery('#pagerEmp'),
        rowNum: 10,
        rowList: [10, 30,30,30,30, 40],
        height: '100%',
        width: '100%',
        caption: 'Tabla departamentos',
        loadtext: "Cargando...",
        emptyrecords: 'No se encontraron datos',
        /*postData: { username: username },*/
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0",

        },
        fixed: true,
        autowidth: true,
        multiselect: false,
        shrinkToFit: true,
        beforeSelectRow: function () { return false; }

    });
    $("#gridEmp").unbind("contextmenu");
    $('.ui-jqgrid-titlebar-close').hide();
    $("#modalEmpleados").on('shown.bs.modal', function () {
        $('#gridEmp').jqGrid('setGridWidth', $(".modalE").width());
        $(".loaderE").fadeOut(500, function () {
            $("#divE").fadeIn(500);
        });

    });
    $(window).on('resize.jqGrid', function () {
        $('#gridEmp').jqGrid('setGridWidth', $(".modalE").width());

    });
    $(document).on('click', '.btnAgregarE', function (e) {
        var id = e.target.id.replace("btnE", "");
        var v = 0;
        for (var i = 0; i < objEmp.length; i++) {
            if (objEmp[i]['empleado'] === id) {
                v++;
                inputEmpleado.value = objEmp[i]['empleado']
                /*descripcionDep.value = objD[i]['nombre'];
                descripcionA.value = objD[i]['area'];
                descripcionD.value = objD[i]['idDepartamento'];*/
                if (v === 1) {
                    $('#modalEmpleados').modal('hide');
                    return;
                }
            }
        }
    });
    /*
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
    });*/
});