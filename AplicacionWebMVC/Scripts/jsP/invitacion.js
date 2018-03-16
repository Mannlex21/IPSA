var objMaterial = [];
$(function () {
    var contadorMaterial=0;
    $('[data-toggle="tooltip"]').tooltip();
    $(document).on('click', '#btnAgregarI', function (e) {
        c = 0;
        $("#divMaterial :input").each(function () {
            console.log($(this).attr('id'));
            if ($(this)[0].value == "") {
                if ($(this).attr('id') !="btnAgregarI"){
                    c++;
                }
            }
        });
        console.log(c);
        if (c == 0) {
            var partidaCotizacion = document.getElementById('partidaCotizacion').value;
            var materialCotizacion = document.getElementById('materialCotizacion').value;
            var cantidadCotizacion = document.getElementById('cantidadCotizacion').value;
            var uMedidaCotizacion = document.getElementById('uMedidaCotizacion').value;
            var descripcionMCotizacion = document.getElementById('descripcionMCotizacion').value;
            var costoCotizacion = document.getElementById('costoCotizacion').value;
            var tiempoEntregaCotizacion = document.getElementById('tiempoEntregaCotizacion').value;
            var notaCotizacion = document.getElementById('notaCotizacion').value;

            contadorMaterial++;
            var partida = {
                "partida": partidaCotizacion,
                "material": materialCotizacion,
                "cantidad": cantidadCotizacion,
                "uMedida": uMedidaCotizacion,
                "descripcion": descripcionMCotizacion,
                "costoU": costoCotizacion,
                "tiempoEntrega": tiempoEntregaCotizacion,
                "nota": notaCotizacion
            };
            objMaterial.push(partida);
            if (c <= 8) {
                agregarElem();
                //limpiarMateriales();
                //desbloquearMateriales();

            } else {
                //btnAgregar.disabled = true;
            }
        }else {
            swal("Error!", "Ocurrio el siguiente error: Hay campos en blanco en material", "error");
        }
    });
    $(document).on('click', '.btnControl', function (e) {
        objMaterial.splice(parseInt(e.target.id.replace("btn", "")), 1);
        contadorMaterial = contadorMaterial - 1;
        agregarElem();
        console.log(objMaterial)
        //bloquearBoton();
    });
    $(document).on('click', '#btnGuardar', function (e) {
        c = 0;
        $("#divInfo :input").each(function () {
            //console.log($(this).attr('id'));
            if ($(this)[0].value=="") {
                c++;
            }
        });
        if (c == 0) {
            var requisicionCotizacion = document.getElementById('requisicionCotizacion').value;
            var departamentoCotizacion = document.getElementById('departamentoCotizacion').value;
            var areaCotizacion = document.getElementById('areaCotizacion').value;
            var cicloCotizacion = document.getElementById('cicloCotizacion').value;
            var proveedorCotizacion = document.getElementById('proveedorCotizacion').value;
            var fechaCotizacion = document.getElementById('fechaCotizacion').value;
            var descripcionCondicion = document.getElementById('descripcionCondicion').value;
            var libreAbordoCotizacion = document.getElementById('libreAbordoCotizacion').value;
            var descuentoCotizacion = document.getElementById('descuentoCotizacion').value;
            var garantiaCotizacion = document.getElementById('garantiaCotizacion').value;
            var anticipoCotizacion = document.getElementById('anticipoCotizacion').value;
            var apoyoTecnicoCotizacion = document.getElementById('apoyoTecnicoCotizacion').value;
            var observacionesCotizacion = document.getElementById('observacionesCotizacion').value;
        } else {
            swal("Error!", "Ocurrio el siguiente error: Hay campos en blanco en información", "error");
        }
    });

});
function Cotizacion(preRequisicion, requisicion, departamento, ejercicio, area, ciclo) {
    console.log(requisicion)
    document.getElementById('requisicionCotizacion').value = requisicion;
    document.getElementById('departamentoCotizacion').value = departamento;
    document.getElementById('cicloCotizacion').value = ciclo;
    document.getElementById('areaCotizacion').value = area;
    $('#modalCotizacion').modal('show');
    $("#modalCotizacion").on('shown.bs.modal', function () {
        //$('#gridDet').jqGrid('setGridWidth', $(".modalB").width());
        $(".loaderC").fadeOut(500, function () {
            $("#divC").fadeIn(500);
            //$("#gridDet").jqGrid().setGridParam({ postData: { preRequisicion: preRequisicion, ejercicio: ejercicio, departamento: departamento }, page: 1 }).trigger('reloadGrid');
        });
    }); 
}
function agregarElem() {
    var tbody = document.getElementById('tbody');
    tbody.innerHTML = '';
    for (var i = 0; i < objMaterial.length; i++) {
        var tr = "<tr>";
        var partida = parseInt(i) + 1;
        tr += "<td style='display:none;'>" + partida + "</td>" +
            "<td>" + parseInt(objMaterial[i]["partida"]) + "</td>" +
            "<td>" + objMaterial[i]["cantidad"] + "</td>" +
            "<td>" + objMaterial[i]["uMedida"] + "</td>" +
            "<td class='textoMaterial'>" + objMaterial[i]["material"] + "</td>" +
            "<td class='textoDescripcion'>" + objMaterial[i]["descripcion"] + "</td>" +
            "<td>" + objMaterial[i]["costoU"] + "</td>" +
            "<td>" + objMaterial[i]["tiempoEntrega"] + "</td>" +
            "<td class='textoNota'>" + objMaterial[i]["nota"] + "</td>" +
            "<td id='ctrl" + i + "'></td></tr>";
        tbody.innerHTML += tr;
        var btn = document.createElement('button');
        var td = document.getElementById('ctrl' + i);
        btn.type = "button";
        btn.id = "btn" + i;
        btn.className = "btnControl btn btn-danger glyphicon glyphicon-trash";
        td.appendChild(btn);
        //total = total + (parseFloat(obj[i]["PrecioU"]) * parseFloat(obj[i]["Cantidad"]));
    }
};