$(function () {
    var objM = [];
    var des, marca, uni, conse, precio, exis;
    document.getElementById("divM").style.display = "none";
    var url = $('#materialesURL').data('request-url');
    $("#grid").jqGrid({
            url: url,
            datatype: 'json',
            mtype: 'Get',
            //table header name   
            colNames: ['Id', 'Descripcion','Marca','Unidad','Consecutivo','Precio Unidad','Existencia','Control'],
            //colModel takes the data from controller and binds to grid  
            colModel: [
                {
                    key: true,
                    name: 'idMaterial',
                    index: 'idMaterial',
                    editable: false,
                    width: 30,
                    resizable: true,
                    sorttype: "int"
                }, {
                    key: false,
                    name: 'descripcion',
                    index: 'descripcion',
                    editable: false,
                    width:200,
                    resizable: true, formatter: function (cellvalue, rowObject) {
                        des = cellvalue;
                        return cellvalue;
                    }
                }, {
                    key: false,
                    name: 'marca',
                    index: 'marca',
                    editable: false,
                    resizable: true, width: 70,
                    formatter: function (cellvalue, rowObject) {
                        marca = cellvalue;
                        return cellvalue;
                    }
                }, {
                    key: false,
                    name: 'uMedida',
                    index: 'uMedida',
                    editable: false,
                    resizable: true, width: 30,
                    formatter: function (cellvalue, rowObject) {
                        uni = cellvalue;
                        return cellvalue;
                    }
                }, {
                    key: false,
                    name: 'consecutivo',
                    index: 'consecutivo',
                    editable: false,
                    resizable: true,
                    width:60,
                    formatter: function (cellvalue, rowObject) {
                        conse = cellvalue;
                        return cellvalue;
                    }
                }, {
                    key: false,
                    name: 'costoProm',
                    index: 'costoProm',
                    editable: false,
                    resizable: true, width: 50,
                    formatter: function (cellvalue, rowObject) {
                        precio = cellvalue;
                        return cellvalue;
                    }
                }, {
                    key: false,
                    name: 'existencia',
                    index: 'existencia',
                    editable: false,
                    resizable: false, width: 50,
                    formatter: function (cellvalue, rowObject) {
                        exis = cellvalue;
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
                            "idMaterial": rowObject.rowId,
                            "descripcion": des,
                            "marca": marca,
                            "unidad": uni,
                            "consecutivo": conse,
                            "precioU": precio,
                            "existencia": exis
                        };
                        objM.push(a);
                        return "<button id=btnC" + rowObject.rowId +" class='btn btn-success glyphicon glyphicon-plus btnAgregarM' />";
                    }
                }],
            viewrecords: true,
            loadonce: false,
            stringResult: true,
            pager: jQuery('#pager'),
            rowNum: 10,
            rowList: [10, 20, 30, 40],
            height: '100%',
            width:'100%',
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
            autowidth:true,
            multiselect: false,
            shrinkToFit: true,
            beforeSelectRow: function () { return false; }
           
    });
    $("#grid").unbind("contextmenu");
    $('.ui-jqgrid-titlebar-close').hide();
    
    $("#modalMateriales").on('shown.bs.modal', function () {
        $('#grid').jqGrid('setGridWidth', $(".modalB").width());
        $(".loaderM").fadeOut(500, function () {
            $("#divM").fadeIn(500);
        });
    });
    $('#btnModal').on('click',  function (e) {
        $('#modalMateriales').modal('show');
    });
    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            $('#modalMateriales').modal('hide');
            $('#modalEmergente').modal('hide');
        }
    });
    $(window).on('resize.jqGrid', function () {
        $('#grid').jqGrid('setGridWidth', $(".modalB").width());
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
        
    })
    $(document).on('click', '.btnAgregarM', function (e) {
        var id = e.target.id.replace("btnC", "");
        for (var i = 0; i < objM.length; i++) {
            if (objM[i]['idMaterial'] === id) {
                limpiarMateriales();
                bloquearMateriales();
                descripcion.value = objM[i]['descripcion'];
                existencia.value = objM[i]['existencia'];
                precioU.value = objM[i]['precioU'];
                clave.value = objM[i]['idMaterial'];
                unidad.value = objM[i]['unidad'];
                $('#modalMateriales').modal('hide');
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