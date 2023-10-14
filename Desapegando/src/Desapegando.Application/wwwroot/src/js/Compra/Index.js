$(document).ready(function () {
    $(".table").on('click', '.fw-semi-bold', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        //var col8 = currentRow.find("td:eq(8)").text()

        //id = $(this).closest('tr').attr('id').val();

        var quantidade = currentRow.find("#quantidade").val()

        // var quantidade = $('#quantidade').val(); // Obtém o valor da quantidade
        var url = currentRow.find("#url").attr('href'); // Obtém a URL atual do link

        // Adicione os parâmetros à URL
        url = url.replace(/(quantidade=)\d+/, 'quantidade=' + quantidade)

        //url += '?quantidade=' + quantidade;

        // Atualize o atributo href com a nova URL
        currentRow.find("#url").attr('href', url);



        //currentRow.find("#btnExcluirCondomino").attr("hidden", true);
        //$("[id=btnExcluirCondomino]").attr("hidden", true);
        //currentRow.find(".spinner-border").removeAttr("hidden");

        //alert(col8);
    });




    $('#quantidade').click(function (e) {
        e.preventDefault(); // Evita que o link seja seguido normalmente

        var quantidade = $('#quantidade').val(); // Obtém o valor da quantidade
        var url = $("#url").attr('href'); // Obtém a URL atual do link

        // Adicione os parâmetros à URL
        url = url.replace(/(quantidade=)\d+/, 'quantidade=' + quantidade)

        //url += '?quantidade=' + quantidade;

        // Atualize o atributo href com a nova URL
        $("#url").attr('href', url);

        // Agora, você pode redirecionar para a nova URL, se necessário
        // window.location.href = url;
    });
});

function ExcluirCondomino(id, prdId) {

    var qtd = $("#quantidade").val();
    var qtdTotal = $("#quantidadeTotal").val();

    $.ajax({
        url: "/Compra/AdicionarComprador/",
        type: 'POST',
        data: { "id": id, "quantidade": qtd, "produtoId": prdId, "quantidadeTotal": qtdTotal },
        success: function (data) {
            if (data == "400") {
                alert("Voce selecionou mais produtos do que foi vendido.")
                location.reload();
            } else if (data == 404) {
                alert("Ocorreu um erro");
                location.reload();
            } else {
                $('#verticallyCentered').modal('show');
            }
        },
        error: function (error) {
            alert(error.statusText);
        }
    });
}