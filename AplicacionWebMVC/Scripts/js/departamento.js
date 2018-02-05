var objDepCargo = [];
$(function () {
    
    var divDep = document.getElementById('divDep');
    var url = $('#departamentos2URL').data('request-url');
    $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {

                if (document.getElementsByClassName('addDepto') != null) {
                    for (var obj in r) {
                        $('#departamento').append($('<option>', {
                            value: r[obj].idDepartamento,
                            text: r[obj].descripcion
                        }));
                    }
                }
                if (document.getElementById('depCargo') != null) {
                    $('#depCargo').append($('<option>', {
                        value: "",
                        text: "Seleccionar"
                    }));
                    $('#depCargo').append($('<option>', {
                        value: "T",
                        text: "TODOS"
                    }));
                    for (var obj in r) {
                        $('#depCargo').append($('<option>', {
                            value: r[obj].idDepartamento,
                            text: r[obj].descripcion
                        }));
                    }
                }

                if (document.getElementById('departamentoS') != null) {
                    for (var obj in r) {
                        $('#departamentoS').append($('<option>', {
                            value: r[obj].idDepartamento,
                            text: r[obj].descripcion
                        }));
                    }    
                }
                
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("No se encontro");
            }
    });
    $('#depCargo').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        if (valueSelected != "T") {
            borrarDato("T")
            var size = Object.keys(objDepCargo).length;
            if (size === 0) {
                agregarDepartamento(valueSelected, optionSelected[0].text);
            } else {
                var val = tieneDato(valueSelected);
                if (val === true) {

                } else {
                    agregarDepartamento(valueSelected, optionSelected[0].text);
                }
            }
        } else {
            document.getElementById("divDep").innerHTML = "";
            objDepCargo = [];
            agregarDepartamento("T", "TODOS");
            
        }
        console.log(objDepCargo);
        document.getElementById("depCargo").selectedIndex = "T"; 
    });
    $(document).on('click', '.labelDeptoBorrar', function (e) {
        borrarDato(e.target.value);
    });
    function agregarDepartamento(valueSelected,textValueSelected) {
        var element = {
            "id": valueSelected,
            "text": textValueSelected
        };
        objDepCargo.push(element);

        var divNuevo = document.createElement("div");
        divNuevo.setAttribute("class", "class");
        var inputTextId = document.createElement("input");
        inputTextId.setAttribute("type", "text");
        inputTextId.setAttribute("value", valueSelected);
        inputTextId.setAttribute("id", "inp-"+valueSelected);
        inputTextId.setAttribute("class", "form-control");
        inputTextId.setAttribute("style", "display:none;");
        var inputTextNombre = document.createElement("label");

        inputTextNombre.appendChild(document.createTextNode(textValueSelected));
        inputTextNombre.setAttribute("value", valueSelected);
        inputTextNombre.setAttribute("class", "labelDepto ");
        inputTextNombre.setAttribute("id", "lb-" + valueSelected);

        var inputTextNombreB = document.createElement("button");
        inputTextNombreB.appendChild(document.createTextNode("X"));
        inputTextNombreB.setAttribute("class", "btn btn-danger btn-sm labelDeptoBorrar");
        inputTextNombreB.setAttribute("value", valueSelected);
        inputTextNombreB.setAttribute("type", "button");
        inputTextNombreB.setAttribute("id", "btnLB-" + valueSelected);

        divNuevo.appendChild(inputTextId);
        divNuevo.appendChild(inputTextNombre);
        divNuevo.appendChild(inputTextNombreB);
        divDep.appendChild(divNuevo)
    }
    function tieneDato(valor) {
        var c= 0;
        Object.keys(objDepCargo).forEach(function (key) {
            if (objDepCargo[key].id == valor) {
                c++;
            }
        });
        if (c != 0) {
            return true;
        } else {
            return false;
        }
    }
    function borrarDato(valor) {
        var v = -1;
        
        Object.keys(objDepCargo).forEach(function (key) {
            if (objDepCargo[key].id == valor) {
                v = key;
            }
        });
        if (v != -1) {
            document.getElementById("btnLB-" + valor).remove();
            document.getElementById("lb-" + valor).remove();
            document.getElementById("inp-" + valor).remove();
            objDepCargo.splice(v, 1);
        }
        
    }

});