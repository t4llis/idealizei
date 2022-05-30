var idIdeia = 0;
var idQuestaoIdeia = 0;
var qtdMensagensCocriador = 0;

$(document).ready(function () {

    $('.btn_add_descricao_cocriador').on('click', function () {

        idQuestaoIdeia = $(this).attr('data-id_questao_ideias');
        idIdeia = $(this).attr('data-id_ideia');

        $("#id_reposta_card").html('');
        $(".comentario_cocriador").html('');

    });

    $('.btn_Modal_add_descricao_cocriador').on('click', function () {
        let descricao = $('.comentario_cocriador').val();
        descricao = descricao.trim();

        let idResposta = $("#id_reposta_card").html();
        if (idResposta == "") {
            AddDescricaoCard(idIdeia, idQuestaoIdeia, descricao);
        } else {
            EditDescricaoCard(idResposta, descricao);
        }
    });

    $('.btn_Modal_excluir_descricao_cocriador').on('click', function () {
        let idResposta = $("#id_reposta_card").html();
        if (idResposta != "") {
            DeleteDescricaoCard(idResposta);
        }
    });

    $('.resposta_cocriador_change').on('click', function () {
        let idResposta = $(this).attr("data-idDesc");
        $("#id_reposta_card").html(idResposta);

        let descCricao = $("#resposta_card_" + $(this).attr("data-count")).html();
        $(".comentario_cocriador").html(descCricao);
    });

    $('.card_editar_cocriacao').on('click', function () {

        $("#id_reposta_card").html('');
        $("#id_userlogado").html('');
        $("#id_userCadastrado").html('');
        $(".comentario_cocriador").val('');

        let textoCoCriacao = $(this).context.children[0].innerText;
        $(".comentario_cocriador").val(textoCoCriacao);

        let userLogado = $(this).attr("data-userConectado");
        let userCadastrado = $(this).attr("data-userCadastada");

        let idResposta = $(this).attr("data-idResposta");
        $("#id_reposta_card").html(idResposta);

        if (userLogado == userCadastrado) {
            $("#id_userlogado").html(userLogado);
            $("#id_userCadastrado").html(userCadastrado);
        }

    });

    $('.btn_Modal_add_descricao_cocriador_Detalhes').on('click', function () {

        let idQuestaoIdeia = $('#id_questao_ideias_card').html();

        let descricao = $('.comentario_cocriador').val();
        descricao = descricao.trim();

        let idResposta = $("#id_reposta_card").html();
        let userLogado = $("#id_userlogado").html();
        let userCadastrado = $("#id_userCadastrado").html();
        idIdeia = $("#id_ideiaSelecionada").html();

        if (idResposta === ""){
            AddDescricaoCard(idIdeia, idQuestaoIdeia, descricao);
        } else {

            if (((userLogado != "") && (userCadastrado != "")) && (userLogado == userCadastrado)) {
                EditDescricaoCard(idResposta, descricao);
            } else {
                Messages('warning', 'ALERTA!', 'Você não tem permissão para editar esse comentário.');
            }
        }
    });

    $('.btn_Modal_excluir_descricao_cocriador_Detalhes').on('click', function () {

        let idResposta = $("#id_reposta_card").html();
        let userLogado = $("#id_userlogado").html();
        let userCadastrado = $("#id_userCadastrado").html();

        if (((userLogado != "") && (userCadastrado != "")) && (userLogado == userCadastrado)) {
            DeleteDescricaoCard(idResposta);
        } else {
            Messages('warning', 'ALERTA!', 'Você não tem permissão para editar esse comentário.');
        }

    });

    $('.btn_add_card_colaborar_cocriacao').on('click', function () {

        $('.comentario_cocriador').val('');
        $("#id_reposta_card").html('');
        $("#id_ideiaSelecionada").html('');

        $("#id_ideiaSelecionada").html($(this).attr('data-id_ideia_Selecionada'));
        $("#id_questao_ideias_card").html($(this).attr('data-id_questao_ideias'));
        
    });

    let idIdeia = $('#id_ideia_selecioanda').html();

    let urlAtual = window.location.href; 
    let urlNecessaria = $URL_PRINCIPAL + 'Ideia/Details/?idIdeia=' + idIdeia;

    if (urlAtual == urlNecessaria) {
        qtdMensagensCocriador = parseInt($('#id_qtd_respostas_cocriadores').html());

        RecarregarInformacoesCoCriador();

        setInterval(RecarregarInformacoesCoCriador, 5000);
    }

});


function AddDescricaoCard(idIdeia, idQuestaoIdeia, desc) {
    Loading.start();

    let ObjTotais = '{';
    ObjTotais += '"ID_IDEIA":"' + idIdeia + '",';
    ObjTotais += '"ID_QUESTAO_IDEIA":"' + idQuestaoIdeia + '",';
    ObjTotais += '"RESPOSTA":"' + desc + '"';
    ObjTotais += '}';
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/AddDescricaoCardCoCriador',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                Messages('success', 'SUCESSO!', 'Adicionado com sucesso.');
                location.reload();
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

function EditDescricaoCard(idResposta, desc) {
    Loading.start();

    let ObjTotais = '{';
    ObjTotais += '"ID":"' + idResposta + '",';
    ObjTotais += '"RESPOSTA":"' + desc + '"';
    ObjTotais += '}';
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/EditDescricaoCardCoCriador',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                Messages('success', 'SUCESSO!', 'Alteração salva com sucesso.');
                location.reload();
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

function DeleteDescricaoCard(idResposta) {
    Loading.start();

    let ObjTotais = '{';
    ObjTotais += '"ID":"' + idResposta + '"';
    ObjTotais += '}';
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/DeleteDescricaoCardCoCriador',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                Messages('success', 'SUCESSO!', 'Descrição Excluida com sucesso.');
                location.reload();
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

function RecarregarInformacoesCoCriador() {
    let idIdeia = $('#id_ideia_selecioanda').html();

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/GetRespostaCoCriadores?idIdeia=' + idIdeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {

                if (parseInt(data) != qtdMensagensCocriador) {
                    document.location.reload(true);
                }

            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

