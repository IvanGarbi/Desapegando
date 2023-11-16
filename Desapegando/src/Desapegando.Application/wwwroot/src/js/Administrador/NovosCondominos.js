$(document).ready(function () {
    $(".table").on('click', '.fw-semi-bold', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        //var col8 = currentRow.find("td:eq(8)").text()

        //currentRow.find("#btnExcluirCondomino").attr("hidden", true);
        $("[id=btnExcluirCondomino]").attr("hidden", true);
        currentRow.find(".spinner-border").removeAttr("hidden");

        //alert(col8);
    });
});

function ExcluirCondomino(id) {
    $.ajax({
        url: "/Administrador/ExcluirCondomino/",
        type: 'POST',
        data: { "id": id },
        success: function (data) {
            if (data.status != "404") {
                location.reload();
            } else {
                alert(data.erro);
                location.reload();
            }
        },
        error: function (error) {
            alert(error.statusText);
        }
    });
}