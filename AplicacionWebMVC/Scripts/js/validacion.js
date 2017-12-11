﻿var prerequisicionG;
var chkBox;
$(function () {
    // Initialize form validation on the registration form.
    // It has the name attribute "registration"
    
    $(document).on("click", "#btnGuardar", function () {
        reestablecerInputsError();
        if ($.isEmptyObject(obj)) {
            $("#errTablaP").text("*Se necesita al menos una partida");
        } else {
            $("#errTablaP").text("");
        }
        $("form[name='infoSol']").validate({
            rules: {
                departamento: "required",
                uso: "required",
                fechaNecesitar: "required",
                fechaActual: "required",
                ejercicio: {required:true,number:true} ,
                partidaPrep: "required"
            },
            errorPlacement: function (error, element) {
                //Custom position: first name
                if (element.attr("name") == "departamento") {
                    $("#errDepartamento").text("*Es requerido");
                }
                if (element.attr("name") == "uso") {
                    $("#errUso").text("*Es requerido");
                }
                if (element.attr("name") == "fechaNecesitar") {
                    $("#errFechaN").text("*Es requerido");
                }
                if (element.attr("name") == "fechaActual") {
                    $("#errFechaA").text("*Es requerido");
                }
                if (element.attr("name") == "ejercicio") {
                    if (error.text().indexOf("number") != -1) {
                        $("#errEjercicio").text("*Debe ser año en numero");
                    }
                    if (error.text().indexOf("required") != -1) {
                        $("#errEjercicio").text("*Es requerido");
                    }
                }
                if (element.attr("name") == "partidaPrep") {
                    $("#errPartidaPrep").text("*Es requerido");
                }
                else {
                    error.append($('.errorTxt span'));
                }
            },
            messages: {
                
            }
        });

        if ($("form[name='infoSol']").valid()) {
            if (!$.isEmptyObject(obj)) {
                guardarSolicitud()
            } else{
                $("#errTablaP").text("*Se necesita al menos una partida");
            }
        } 
    });
    function guardarSolicitud() {
        
        var obs = document.getElementById("observaciones").value;
        var data = {
            idRequisicion: "n/a",
            fechaRequisicion: document.getElementById("fechaActual").value,
            uso: document.getElementById("uso").value,
            fechaNecesitar: document.getElementById("fechaNecesitar").value,
            departamento: document.getElementById("departamentoD").value,
            ciclo: document.getElementById("ciclo").value,
            area: document.getElementById("departamentoA").value,
            fechaRecepcion: "01/01/2017",
            observaciones:obs,
            ejercicio: document.getElementById("ejercicio").value,
            solicitante: 0
        };
        chkBox= {
            trabajoSindicato: $("#chkB1").is(':checked'),
            retencionImpuesto: $("#chkB2").is(':checked'),
            altura: $("#chkB3").is(':checked'),
            espaciosConfinados: $("#chkB4").is(':checked'),
            electrico: $("#chkB5").is(':checked'),
            corte: $("#chkB6").is(':checked'),
            soldadura: $("#chkB7").is(':checked'),
            izajes: $("#chkB8").is(':checked'),
            montacarga: $("#chkB9").is(':checked'),
        };
        var d = {
            solicitud: data,
            partidas: obj,
            checks: chkBox
        }
        var url = $('#requisicionURL').data('request-url');
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(d),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            beforeSend: function () {
                $(".loader").fadeIn(1);
            },
            complete: function () {
                
            },
            success: function (result) {
                prerequisicionG = result.preRequisicion;
                document.getElementById("btnImprimir").disabled = false;
                var d = {
                    Subject: "Se agrego una pre-requisicion",
                    fechaNecesitar: data.fechaNecesitar,
                    fechaRequisicion: data.fechaRecepcion,
                    uso: data.uso,
                    observaciones: data.observaciones,
                    departamento: document.getElementById("departamento").value
                };
                var url = $('#emailURL').data('request-url');
                $.ajax({
                    type: "POST",
                    url: url,
                    data: JSON.stringify(d),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (result) {
                        console.log("Se envio mail");
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log("No se envio mail");
                    }
                });
                $(".loader").fadeOut(100);
                swal("Guardado!", "Se ha guardado la solicitud correctamente", "success");   
            }
        });
       
    }
});
function reestablecerInputsError() {
    $("#errDepartamento").text("");
    $("#errFechaN").text("");
    $("#errUso").text("");
    $("#errFechaA").text("");
    $("#errEjercicio").text("");
    $("#errPartidaPrep").text("");
    $("#errObservaciones").text("");
    $("#errDescripcion").text("");
    $("#errCantidad").text("");
    $("#errUnidad").text("");
    $("#errExistencia").text("");
    $("#errClave").text("");
    $("#errPrecioU").text("");
    $("#errDetalle").text("");
    $("#errTablaP").text("");
}
function removeClass(node, cls) {
    if (node && node.className && node.className.indexOf(cls) >= 0) {
        var pattern = new RegExp('\\s*' + cls + '\\s*');
        node.className = node.className.replace(pattern, ' ');
    }
}