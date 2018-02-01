var descripcion = document.getElementById("descripcion");
var cantidad = document.getElementById("cantidad");
var unidad = document.getElementById("unidad");
var existencia = document.getElementById("existencia");
var clave = document.getElementById("clave");
var detalle = document.getElementById("detalle");
var precioU = document.getElementById("precioU");
var ejercicio= document.getElementById("ejercicio");
var obj = [];
var total = 0;
var identificacion="no tiene",version=0,fecha="01/01/2017"
$(document).ready(function () {
    /*-------------------------------------------------------Declaracion de variables---------------------------------------------------------------*/
    var c = 0;
    var btnAgregar = document.getElementById("btnAgregarI");
    var jsonMaterial;
    btnAgregar.disabled = true;
    /*-------------------------------------------------------Declaracion de variables---------------------------------------------------------------*/


    /*--------------------------------------------------------Declaracion de Escuchas----------------------------------------------------------------*/
    $(".inputBox2").change(function () {
        bloquearBoton();
    });
    $(".inputBox2").keyup(function () {
        bloquearBoton();
    });
    $("form[name='infoPar']").validate({
        rules: {
            cantidad: { required: true, number: true },
            descripcion: "required",
            unidad: "required",
            existencia: "required",
            clave: "required",
            precioU: { required: true, number: true }
        },
        errorPlacement: function (error, element) {
            //Custom position: first name
            if (element.attr("name") === "cantidad") {
                if (error.text().indexOf("number") !== -1) {
                    $("#errCantidad").text("*Debe ser un numero");
                }
                if (error.text().indexOf("required") !== -1) {
                    $("#errCantidad").text("*Es requerido");
                }
            }
            if (element.attr("name") === "descripcion") {
                $("#errDescripcion").text("*Es requerido");
            }
            if (element.attr("name") === "unidad") {
                $("#errUnidad").text("*Es requerido");
            }
            if (element.attr("name") === "existencia") {
                $("#errExistencia").text("*Es requerido");
            }
            if (element.attr("name") === "clave") {
                $("#errClave").text("*Es requerido");
            }
            if (element.attr("name") === "precioU") {
                if (error.text().indexOf("number") !== -1) {
                    $("#errPrecioU").text("*Debe ser un numero");
                }
                if (error.text().indexOf("required") !== -1) {
                    $("#errPrecioU").text("*Es requerido");
                }
            }
            // 
            // Default position: if no match is met (other fields)
            else {
                error.append($('.errorTxt span'));
            }
        },
        // Specify validation error messages
        messages: {

        }
    });
    $("#btnAgregarI").click(function (e) {
        e.preventDefault();
        if ($("form[name='infoPar']").valid()) {
            var d;
            c = c + 1;
            if (detalle.value === null || detalle.value === "") {
                d = "---";
            } else {
                d = detalle.value;
            }
            var partida = {
                "Descripcion": descripcion.value,
                "Cantidad": cantidad.value,
                "Unidad": unidad.value,
                "Existencia": existencia.value,
                "Clave": clave.value,
                "Detalle": d,
                "PrecioU": precioU.value
            };
            obj.push(partida);
            if (c <= 8) {
                agregarElem();
                limpiarMateriales();
                desbloquearMateriales();

            } else {
                btnAgregar.disabled = true;
            }
        } 
    });
    $(document).on('click', '.btnControl', function (e) {
        obj.splice(parseInt(e.target.id.replace("btn", "")), 1);
        c = c - 1;
        agregarElem();
        bloquearBoton();
    });
    $(document).on('click', '#btnLimpiarFile', function (e) {
        var url = $('#emailURL').data('request-url');
        var d = {
            Subject: "Se agrego una pre-requisicion",
            fechaNecesitar: document.getElementById("fechaNecesitar").value,
            fechaRequisicion: document.getElementById("fechaActual").value,
            uso: document.getElementById("uso").value,
            observaciones: document.getElementById("observaciones").value,
            departamento: document.getElementById("departamento").value
        };
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
                console.log(XMLHttpRequest);
                console.log(errorThrown)
                console.log("No se envio mail");
            }
        });
        //document.getElementById("file1").value = "";
    });
    
    $(document).on("click", "#btnLimpiarP", function () {
        limpiarSolicitante();
    });
    $(document).on("click", "#btnLimpiarI", function () {
        limpiarMateriales();
        desbloquearMateriales();
    });
    $('#tablaPartida').on('click', 'td.textoDetalle', function (e) {
        var text = document.getElementById('textoTabla');
        text.innerHTML= e.target.textContent;
        $('#modalEmergente').modal('show'); 
    });
    $("#btnCancelar").click(function (e) {
        window.location = '/';
    });
    /*--------------------------------------------------------Declaracion de Escuchas----------------------------------------------------------------*/ 
    /*Se inicializa la tabla de Partida*/
    var url = $('#ejercicioURL').data('request-url');
    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            ejercicio.value = result.ejercicio.ejercicio;
            identificacion = result.ejercicio.identificacion;
            fecha = ToJavaScriptDate(result.ejercicio.fecha);
            version = result.ejercicio.version;
        }, error: function () {
            
        }
    });
    var table = $('#tablaPartida').DataTable({
        responsive: true, paging: false, searching: false,
        "columnDefs": [
            { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] },
            { "searchable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }
            
        ], "bSort": false,
        "sEmptyTable": "Aun no hay datos."
    });
    /*--------------------------------------,--------------------------------FUNCIONES----------------------------------------------------------------*/
    function inicializarTablaMaterial() {
        var table = $('#tablaAlmacen').DataTable({
            responsive: true, searching: false, iDisplayLength: 8, bResetDisplay:true,
            "columnDefs": [
                { "orderable": false, "targets": [0, 1, 2, 3, 4] },
                { "searchable": false, "targets": [0, 1, 2, 3, 4] }
            ], "bSort": false
        });
        
    }
    function bloquearBoton() {

        if (esVacio() === true) {
            btnAgregar.disabled = true;
        } else {
            if (c === 8) {
                btnAgregar.disabled = true;
            } else {
                btnAgregar.disabled = false;
            }
        }
    };
    function esVacio() {
        if (descripcion.value === "" || descripcion.value === null || cantidad.value === "" || cantidad.value === null ||
            unidad.value === "" || unidad.value === null || existencia.value === "" || existencia.value === null ||
            clave.value === "" || clave.value === null || /*detalle.value === "" || detalle.value === null ||*/ precioU.value === "" ||
            precioU.value === "") {
            return true;
        } else {
            return false;
        }
    };
    function agregarElemM() {
        var tbody = document.getElementById('tbody2');
        tbody.innerHTML = '';
        for (var i = 0; i < jsonMaterial.length; i++) {
            var tr = "<tr>";
            tr += "<td>" + jsonMaterial[i]["idMaterial"] + "</td>" +
                "<td>" + jsonMaterial[i]["descripcion"] + "</td>" +
                "<td>" + jsonMaterial[i]["existencia"] + "</td>" +
                "<td>" + jsonMaterial[i]["costoProm"] + "</td>" +
                "<td id='ctrlM" + jsonMaterial[i]["idMaterial"] + "'></td></tr>";
            tbody.innerHTML += tr;
            var btn = document.createElement('button');
            var td = document.getElementById('ctrlM' + jsonMaterial[i]["idMaterial"]);
            btn.type = "button";
            btn.id = "btnM" + jsonMaterial[i]["idMaterial"];
            btn.className = "btn btn-success glyphicon glyphicon-plus btnAgregarMaterial";
            td.appendChild(btn);
        }
    };
    function agregarElem() {
        var tbody = document.getElementById('tbody');
        var idTotal = document.getElementById("idTotal");
        tbody.innerHTML = '';
        idTotal.innerHTML = "";
        total = 0;
        for (var i = 0; i < obj.length; i++) {
            var tr = "<tr>";
            var partida = parseInt(i) + 1;
            tr += "<td>" + partida + "</td>" +
                "<td>" + obj[i]["Cantidad"] + "</td>" +
                "<td>" + obj[i]["Unidad"] + "</td>" +
                "<td class='textoDetalle'>" + obj[i]["Descripcion"] + "</td>" +
                "<td>" + obj[i]["Clave"] + "</td>" +
                "<td>" + obj[i]["Existencia"] + "</td>" +
                "<td class='textoDetalle'>" + obj[i]["Detalle"] + "</td>" +
                "<td>$ " + obj[i]["PrecioU"] + "</td>" +
                "<th>$ " + (parseFloat(obj[i]["PrecioU"]) * parseFloat(obj[i]["Cantidad"])) + "</th>" +
                "<td id='ctrl" + i + "'></td></tr>";
            tbody.innerHTML += tr;
            var btn = document.createElement('button');
            var td = document.getElementById('ctrl' + i);
            btn.type = "button";
            btn.id = "btn" + i;
            btn.className = "btnControl btn btn-danger glyphicon glyphicon-trash";
            td.appendChild(btn);
            total = total + (parseFloat(obj[i]["PrecioU"]) * parseFloat(obj[i]["Cantidad"]));
        }
        idTotal.innerHTML = "$ "+total;
    };
    function buscarElem(general, nombre, clave) {
        if ((general === null || general === "" || general === undefined) && (nombre === null || nombre === "" || nombre === undefined)
            && (clave === null || clave === "" || clave === undefined)) {
            agregarElemM();
            $('#tablaAlmacen').DataTable().page.len(8).draw();
        } else {
            var tbody = document.getElementById('tbody2');
            tbody.innerHTML = '';
            for (var i = 0; i < jsonMaterial.length; i++) {

                if (jsonMaterial[i]["clave"].toString().toLowerCase().match(clave.toLowerCase()) && jsonMaterial[i]["descripcion"].toString().toLowerCase().match(nombre.toLowerCase())
                    && (jsonMaterial[i]["clave"].toString().toLowerCase().match(general.toLowerCase()) || jsonMaterial[i]["descripcion"].toString().toLowerCase().match(general.toLowerCase())
                        || jsonMaterial[i]["existencia"] === general || jsonMaterial[i]["precioU"] === general)) {
                    var tr = "<tr>";
                    tr += "<td>" + jsonMaterial[i]["clave"] + "</td>" +
                        "<td>" + jsonMaterial[i]["descripcion"] + "</td>" +
                        "<td>" + jsonMaterial[i]["existencia"] + "</td>" +
                        "<td>" + jsonMaterial[i]["precioU"] + "</td>" +
                        "<td id='ctrlM" + jsonMaterial[i]["idMaterial"] + "'></td></tr>";
                    tbody.innerHTML += tr;
                    var btn = document.createElement('button');
                    var td = document.getElementById('ctrlM' + jsonMaterial[i]["idMaterial"]);
                    btn.type = "button";
                    btn.id = "btnM" + jsonMaterial[i]["idMaterial"];
                    btn.className = "btn btn-success glyphicon glyphicon-plus btnAgregarMaterial";
                    td.appendChild(btn);
                }
            }
        }

    }
    function limpiarSolicitante() {
        $(".inputBox").val('');
        $("#ciclo").val('Z');
        $("#fechaActual").val(fechaAct);
    }
    function desbloquearMateriales() {
        descripcion.readOnly = false;
        unidad.readOnly = false;
        existencia.readOnly = false;
        clave.readOnly = false;
        precioU.readOnly = false;
        btnAgregar.disabled = true;
    }
    function abrirPantallaCarga() {
        var divC = document.createElement("div");
        var x = document.getElementsByTagName("BODY")[0];
        divC.setAttribute("class", "modal-backdrop fade in");
        divC.innerHTML = '<svg width = "1000" height= "1000" viewBox= "0 0 1000 1000" xmlns = "http://www.w3.org/2000/svg" version= "1.1"xmlns: xlink = "http://www.w3.org/1999/xlink" ><g transform="translate(500,500)"><rect class="rotate-45 rotate-back" x=-5 y=-5 width=10 height=10 stroke="white" stroke-width=20 fill="none" /><rect class="rotate-45 rotate" x=-50 y=-50 width=100 height=100 stroke="white" stroke-width=20 stroke-linejoin="bevel" fill="none" /><g transform="translate(-50,0) rotate(-45)"><polyline class="left" points="40,-40 50,-50 -40,-50 -50,-40 -50,50 -40,40" stroke="white" stroke-width=20 fill="none" /></g><g transform="translate(50,0) rotate(135)"><polyline class="right" points="40,-40 50,-50 -40,-50 -50,-40 -50,50 -40,40" stroke="white" stroke-width=20 fill="none" /></g><text y=160 text-anchor="middle" font-weight="bold" font-size="3em" font-family="sans-serif" fill="white">Cargando...</text></g></svg >';
        x.appendChild(divC);
    }
    function cerrarPantallaCarga() {
        $(".modal-backdrop").remove();
    }
    
    /*----------------------------------------------------------------------FUNCIONES----------------------------------------------------------------*/
});
function limpiarMateriales() {
    $(".inputBox2").val('');
}
function bloquearMateriales() {
    descripcion.readOnly = true;
    unidad.readOnly = true;
    existencia.readOnly = true;
    clave.readOnly = true;
    precioU.readOnly = true;
}
function ToJavaScriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    if (dt.getDate() <10 ){
        dia = "0" + dt.getDate();
    } else {
        dia = dt.getDate();
    }
    if ((dt.getMonth() + 1)<10){
        mes = "0"+(dt.getMonth() + 1);
    } else {
        mes = (dt.getMonth() + 1);
    }
    return (dia + "/" + mes + "/" + dt.getFullYear());
}