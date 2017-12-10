$(function () {
    $("#gridDet").unbind("contextmenu");
    $('.ui-jqgrid-titlebar-close').hide();
    $(window).on('resize.jqGrid', function () {
        $('#gridDet').jqGrid('setGridWidth', $(".modalD").width());

    });
});
function abrirDetalle(preRequisicion, departamento, ejercicio,uso,ciclo) {
    var d = {
        preRequisicion: preRequisicion,
        departamento: departamento,
        ejercicio: ejercicio

    };
    $('#modalDetalle').modal('show');
    document.getElementById("lblPreReq").innerHTML = preRequisicion;
    document.getElementById("lblUsoR").innerHTML = uso;
    document.getElementById("lblCiclo").innerHTML = ciclo;
    if ($("#gridDet").is(':empty')) {

        $("#modalDetalle").on('shown.bs.modal', function () {
            $('#gridDet').jqGrid('setGridWidth', $(".modalB").width());
            $(".loaderD").fadeOut(500, function () {
                $("#divD").fadeIn(500);
                var url = $('#solicitudURL').data('request-url');
               $("#gridDet").jqGrid({
                    url: url,
                    datatype: 'json',
                    contentType: "application/json; charset=utf-8",
                    postData: { preRequisicion: preRequisicion, ejercicio: ejercicio, departamento: departamento },
                    mtype: 'POST', 
                    colNames: ['Id', 'Partida', 'Id', 'Descripcion', 'Detalle','Costo unitario','Cantidad'],
                    colModel: [
                        {
                            key: true,
                            name: 'idDReq',
                            index: 'idDReq',
                            editable: false,
                            width: 30,
                            resizable: true,
                            hidden: true
                        },  {
                            key: false,
                            name: 'partida',
                            index: 'partida',
                            editable: false,
                            width: 60,
                            resizable: true, formatter: function (cellvalue, rowObject) {
                                return parseInt(cellvalue)+ 1;
                            }
                        }, {
                            key: false,
                            name: 'idMaterial',
                            index: 'idMaterial',
                            editable: false,
                            width: 65,
                            resizable: true, formatter: function (cellvalue, rowObject) {
                                return cellvalue;
                            }
                        },  {
                            key: false,
                            name: 'descripcion',
                            index: 'descripcion',
                            editable: false,
                            width: 500,
                            resizable: true, formatter: function (cellvalue, rowObject) {
                                return cellvalue;
                            }
                        }, {
                            key: false,
                            name: 'detalle',
                            index: 'detalle',
                            editable: false,
                            width: 350,
                            resizable: true, formatter: function (cellvalue, rowObject) {
                                return cellvalue;
                            }
                        }, {
                            key: false,
                            name: 'costoU',
                            index: 'costoU',
                            editable: false,
                            width: 120,
                            resizable: true, formatter: function (cellvalue, rowObject) {
                                return cellvalue;
                            }
                        }, {
                            key: false,
                            name: 'cantidad',
                            index: 'cantidad',
                            editable: false,
                            width: 80,
                            resizable: true, formatter: function (cellvalue, rowObject) {
                                return cellvalue;
                            }
                        }],
                    viewrecords: true,
                    loadonce: false,
                    stringResult: true,
                    pager: jQuery('#pagerDet'),
                    rowNum: 10,
                    rowList: [10, 20, 30, 40],
                    height: '100%',
                    width: '100%',
                    caption: '',
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
            });
        });
    } else {
       $(".loaderD").fadeOut(500, function () {
            $("#divD").fadeIn(500);
            $("#gridDet").jqGrid().setGridParam({ postData: { preRequisicion: preRequisicion, ejercicio: ejercicio, departamento: departamento }, page: 1 }).trigger('reloadGrid');
        });
        
    }
    
}