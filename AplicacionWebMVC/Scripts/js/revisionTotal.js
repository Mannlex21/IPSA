$(function () {
    $(document).on("click", "#btnLimpiarSearch", function () {
        $(".inputBox").val('');
        document.getElementById('cicloS').value = ''; 
        document.getElementById('departamentoS').value = '';
    });
});