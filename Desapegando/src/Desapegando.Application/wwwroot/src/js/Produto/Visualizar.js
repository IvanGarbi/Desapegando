$(document).ready(function () {
    $("#btnRemoverProduto").on('click', function () {

        // Example starter JavaScript for disabling form submissions if there are invalid fields
        (() => {
            'use strict'

            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            const forms = document.querySelectorAll('.needs-validation')

            // Loop over them and prevent submission
            Array.from(forms).forEach(form => {
                form.addEventListener('submit', event => {
                    if (!form.checkValidity()) {
                        event.preventDefault()
                        event.stopPropagation()
                    }

                    form.classList.add('was-validated')
                }, false)
            })
        })()

        if ($("#validationCustom03").val()) {
            RemoverProduto($("#produtoId").val())
        }
    });
});

function RemoverProduto(id) {
    $.ajax({
        url: "/Produto/RemoverProduto/",
        type: 'POST',
        data: {
            "id": id,
            "motivo": $("#validationCustom03").val()
        },
        success: function (data) {
            if (data.status != "404") {
                location.reload();
            } else {
                alert(data.erro);
                location.reload();
            }
        },
        //error: function (error) {
        //    alert(error.statusText);
        //}
    });
}