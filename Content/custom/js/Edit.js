$(document).ready(function () {

    let id_ideia = $('#id_ideia').html();
    let urlAtual = window.location.href;
    if ((urlAtual == $URL_PRINCIPAL + 'Ideia/Edit?idIdeia=0&TipoIdeia=') ||
        (urlAtual.includes($URL_PRINCIPAL + 'Ideia/Edit?idIdeia=0&TipoIdeia='))) {

        let hash = GetHashId(id_ideia);
        window.location.href = $URL_PRINCIPAL + 'Ideia/Edit?idIdeia=' + hash;
    }
    else if (urlAtual.includes($URL_PRINCIPAL + 'Ideia/Edit?'))
    {
        // Tags modal solução
        $('#tagsSolucoes').selectize({
            plugins: ['remove_button'],
            delimiter: ',',
            persist: false,
            create: function (input) {
                return {
                    value: input,
                    text: input
                }
            }
        });

        CarregarInformacoesCabecalho();
        CarregarDescricaoProblema();
        CarregarSolucoes();
        CarregarRespostas();
        CarregarTodosCardsCoCriacao();
        CarregarAnexos();

        $('.concluir-ideia-button').on('click', function () {
            
            if (!isTagSolucaoSelecionado()) {
                return;
            }

            Loading.start();
            if (!VerificarRespostas()) {
                Messages('warning', 'ATENÇÃO!', 'A respostas para as perguntas são obrigatórias!');
                Loading.done();
                return;
            }
            Loading.done();

            let id_ideia = $('#id_ideia').html();
            if ($('#nome_ideia').html().trim() == "") {
                Messages('warning', 'ATENÇÃO!', 'Informe o nome da ideia, para ser concluida!');
            } else {
                window.location.href = $URL_PRINCIPAL + "Ideia/Share/?idIdeia=" + id_ideia+"&shareEnum=AVALIACAO";
            }
            

            let link_share = $URL_PRINCIPAL + "Ideia/CoCriador/?idIdeia=" + id_ideia;
            $('#input_copy_links').val(link_share);
            $('#input_copy_links').select();
            document.execCommand('copy');
        });

        $('#btn-copiar-link').on('click', function () {
            let id_ideia = $('#id_ideia').html();
            let link_share = $URL_PRINCIPAL + "Ideia/CoCriador/?idIdeia=" + id_ideia;
            $('#input_copy_links').val(link_share);
            $('#input_copy_links').select();
            document.execCommand('copy');
        });

        $('#btn-enviar-emails').on('click', function () {           
            const emails = window.emailsInput.getValue();
            EnviarEmail(emails);
        });       

        $('#btn-SalvarDados-cabecalho').on('click', function () {
            SalvarCabecalho();
        });

        $('.resposta_pergunta_click').on('click', function () {
            let id_pergunta = $(this).attr("data-idpergunta");

            if (!isPodeAbrirModalRespostaPergunta(id_pergunta)) {
                return;
            }            

            $('#id_questao_modal').html(id_pergunta);
            $('#id_ideia_modal').html($('#id_ideia').html());

            let textoCard = $('#resposta_pergunta_' + id_pergunta).html();

            textoCard = textoCard.trim();
            if (textoCard != "Clique aqui para responder") {
                $('#InputTextoRespostaPergunta').val(textoCard);
            } else {
                $('#InputTextoRespostaPergunta').val('');
            }


        });

        $('#btn-Salvar-Resposta').on('click', function () {
            CadastrarRespostaPergunta();
        });

        $('.cards_colaboracao_img').on('click', function () {

            $('.comentario_cocriador').val('');
            $('#id_card_cocriacao').html('');
            $('#id_userCadastrado').html('');

            $('#id_pergunta').html($(this).attr("data-idpergunta"));

        });

        $('.Modal_btn_Add_descricao_cocriador').on('click', function () {

            let descricao = $('.comentario_cocriador').val();
            descricao = descricao.trim();
            descricao = descricao.replaceAll('"', "'");

            let idIdeia = $('#id_ideia').html();
            let idPergunta = $('#id_pergunta').html();
            let idCard = $('#id_card_cocriacao').html();
            let userLogado = $('#id_userLogado').html();
            let userCadastrado = $('#id_userCadastrado').html();


            if (idCard === "") {
                AddCard(idIdeia, idPergunta, descricao);
            } else {

                if (((userLogado != "") && (userCadastrado != "")) && (userLogado == userCadastrado)) {
                    EditCard(idIdeia, idCard, idPergunta, descricao);
                } else {
                    Messages('warning', 'ALERTA!', 'Você não tem permissão para editar esse comentário.');
                }
            }

        });

        $('.Modal_btn_excluir_descricao_cocriador').on('click', function () {
            let idIdeia = $('#id_ideia').html();
            let idPergunta = $('#id_pergunta').html();
            let idCard = $('#id_card_cocriacao').html();
            let userLogado = $('#id_userLogado').html();
            let userCadastrado = $('#id_userCadastrado').html();

            if (((userLogado != "") && (userCadastrado != "")) && (userLogado == userCadastrado)) {
                DeleteCard(idIdeia, idPergunta, idCard);
            } else {
                Messages('warning', 'ALERTA!', 'Você não tem permissão para editar esse comentário.');
            }

        });


        $('.btn-salvar-novo-video-pit').on('click', function () {
            
            let linkNovoPit = $('#link-novo-pit').val();
            let idIdeia = $('#id_ideia').html();

            if (isNotValidUrl(linkNovoPit)) {
                Messages('warning', 'ATENÇÃO!', 'Link do pit inválido!');
                return;
            }

            let ObjTotais = '{';
            ObjTotais += '"idieia":"' + idIdeia + '",';
            ObjTotais += '"linkPit":"' + linkNovoPit + '"';
            ObjTotais += '}';
            $.ajax(
                {
                    type: 'POST',
                    url: $URL_PRINCIPAL + 'Ideia/EditLinkPit/',
                    data: ObjTotais,
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    datatype: 'json',
                    beforeSend: function (xhr) {

                    },
                    success: function (data) {
                        
                        Messages('success', 'SUCESSO!', 'Alteração salva com sucesso.');
                        document.getElementById('id_container_iframe').innerHTML = '';
                        let frame = document.createElement('iframe');
                        frame.width = '100%';
                        frame.height = '300px';
                        frame.src = data;
                        frame.frameBorder = 0;

                        document.getElementById('id_container_iframe').appendChild(frame);

                    },
                    error: function (error) {
                        Loading.done();
                        Messages('error');
                    }
                });

        });

        $('#btn_img_alterar_cabecalho').on('click', function () {
            $('#InputTextoIdeia').val($('#nome_ideia').html());            
        });

        let urlAtual = window.location.href;
        let urlNecessaria = $URL_PRINCIPAL + 'Ideia/Edit?idIdeia=' + id_ideia;

        if (urlAtual == urlNecessaria) {    
            RecarregarInformacoesCoCriador;
            setInterval(RecarregarInformacoesCoCriador, 5000);
        }

        $('.descricao_pergunta_click').on('click', function () {
            let textoDor = $('#dor_ideia').html();

            textoDor = textoDor.trim();
            if (textoDor != "Clique aqui para responder") {
                $('#InputTextoDor').val(textoDor);
            } else {
                $('#InputTextoDor').val('');
            }            
        });

        $('#btn-SalvarDados-descricao-problema').on('click', function () {
            let textoDor = $('#InputTextoDor').val();
            if (textoDor == '') {
                $('#InputTextoDor').focus();
                Messages('warning', 'ATENÇÃO!', 'Obrigatório informar a descrição do problema!');
                return;
            }
            
            SalvarDescricaoProblema();
        });        

        $('#btn-SalvarDados-solucoes').on('click', function () {
           SalvarSolucoes();
        });

        $('.solucoes_click').on('click', function () {
            CarregarSolucoesModal();
        });

        $('.solucao_ideia_tags_click').on('click', function () {
            SalvarSolucaoProjeto();
            CarregarSolucoes();
            
        });
               
    }

});

function isPodeAbrirModalRespostaPergunta(id_pergunta) {    
    let imgEdit = document.getElementById('edit_id_' + id_pergunta);    
    let respostaEdit = document.getElementById('resposta_pergunta_' + id_pergunta);

    if (imgEdit == undefined || imgEdit == null) {
        return;
    }

    if (!isTagSolucaoSelecionado()) {
        imgEdit.removeAttribute("data-toggle");
        imgEdit.removeAttribute("data-target");

        respostaEdit.removeAttribute("data-toggle");
        respostaEdit.removeAttribute("data-target");
        return false;
    }

    imgEdit.setAttribute("data-toggle", "modal");
    imgEdit.setAttribute("data-target", "#Modal_Resposta_Pergunta");

    respostaEdit.setAttribute("data-toggle", "modal");
    respostaEdit.setAttribute("data-target", "#Modal_Resposta_Pergunta");

    return true;
}

function isTagSolucaoSelecionado() {
    let idTagSolucao = $('.chip--active').attr("data-id-tag");

    if (idTagSolucao == undefined || idTagSolucao == null) {
        Messages('warning', 'ATENÇÃO!', 'Obrigatório selecionar uma tag da ideia que pode solucionar o problema!');        
        return false;
    }

    return true;
}

function isNotValidUrl(userInput) {
    var res = userInput.match(/(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g);
    if (res == null)
        return true;
    else
        return false;
}

function EnviarEmail(emails) {

    Loading.start();

    let id_ideia = $('#id_ideia').html();
    let ObjHash = '{';
    ObjHash += '"ID":"' + id_ideia + '"';
    ObjHash += '}';

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/GetHashId?id=' + id_ideia,
            //data: ObjHash,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
            },
            success: function (data) {
                
                if (data != "") {

                    let ObjEmail = '{';
                    let caminho_compartilhar = $URL_PRINCIPAL + "Ideia/CoCriador/?idIdeia=" + data;

                    ObjEmail += '"EMAIL_DESTINATARIO":"' + emails + '",';
                    let tipo_pagina = 'COCRIACAO';

                    ObjEmail += '"ASSUNTO":"' + "Link cocriador Idealizei" + '",';
                    ObjEmail += '"ID_IDEIA":"' + id_ideia + '",';
                    ObjEmail += '"TIPO_ENVIO":"' + tipo_pagina + '",';
                    ObjEmail += '"MENSAGEM":"' + "Olá, \n Você foi escolhido por {0} para cocriar uma ideia na Plataforma(?) Idealizei. \n " +
                        "Sua ajuda é muito importante para {0}.\n " +
                        "Acesse  " + caminho_compartilhar + " e ajude a idealizar essa ideia. \n " +
                        "E-mail automático, não retorne." + '"';

                    ObjEmail += '}';

                    $.ajax(
                        {
                            type: 'POST',
                            url: $URL_PRINCIPAL + 'Ideia/EnviarEmail',
                            data: ObjEmail,
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            datatype: 'json',
                            beforeSend: function (xhr) {
                            },
                            success: function (data) {
                                Loading.done();
                                if (data) {
                                    Messages('success', 'SUCESSO!', 'E-mail enviado com sucesso!');
                                    $('.btn-fechar-modal').click();
                                }
                                else
                                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao tentar enviar seu e-mail, verifique o destinatario.');
                            },
                            error: function () {
                                Loading.done();
                                Messages('error');
                            }
                        });

                }
                else
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao tentar enviar seu e-mail, verifique o destinatario.');
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });

}

function CarregarInformacoesCabecalho() {

    let id_ideia = $('#id_ideia').html();
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarCabecalhoIdeia?idIdeia=' + id_ideia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_cabecalho').attr("style", "display:block;");
            },
            success: function (data) {

                $('#nome_ideia').html(data.NOMEIDEIA);                
                $('#lider_ideia').html(data.LIDER);
                $('#cocriadores_ideia').html(data.COCRIADORES);

                $('#img_loading_cabecalho').attr("style", "display:none;");
            },
            error: function () {
                Messages('error');
            }
        });
}

function CarregarDescricaoProblema() {
    let id_ideia = $('#id_ideia').html();
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarDescricaoProblema?idIdeia=' + id_ideia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_descricao_problema').attr("style", "display:block;");
            },
            success: function (data) {

                if (data.DOR != "") {
                    $('#dor_ideia').html(data.DOR);
                }

                $('#img_loading_descricao_problema').attr("style", "display:none;");
            },
            error: function () {
                Messages('error');
            }
        });
}

function CarregarSolucoes() {
    let id_ideia = $('#id_ideia').html();
    let status_ideia = $('#status_ideia').html();

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarSolucoes?idIdeia=' + id_ideia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_solucoes').attr("style", "display:block;");
            },
            success: function (data) {
                
                // Limpa tags tela principal  
                $('.chip_solucao').remove();

                let div2 = document.createElement("div");
                let elementI2 = document.createElement("i");
                let span5 = document.createElement("span");
                let span6 = document.createElement("span");
                
                for (var i = 0; i < data.length; i++) {
                    
                    // Add tags tela principal
                    let div = document.createElement("div");
                    
                    if (data[i].ID == data[i].ID_SOLUCAO_SELECIONADO) {

                        div.className = 'chip_solucao solucao_ideia_tags_click chip--active';

                    } else {
                        div.className = 'chip_solucao solucao_ideia_tags_click';
                    }

                    div.setAttribute("data-id-tag", data[i].ID);

                    if (parseInt(status_ideia) == 2) {
                        div.setAttribute("style", "cursor: default;")
                        div.setAttribute("data-readonly", true);
                    }

                    let span1 = document.createElement("span");
                    span1.className = "icon icon--leadind chip--check";

                    let elementI = document.createElement("i");
                    elementI.className = 'fa fa-check'

                    span1.appendChild(elementI);

                    let span2 = document.createElement("span");
                    span2.textContent = data[i].SOLUCAO;

                    div.appendChild(span1);
                    div.appendChild(span2);
                    
                    $('.solucao_ideia_tags').append(div);

                    if (data[i].ID == data[i].ID_SOLUCAO_SELECIONADO) {

                        div2.className = 'chip_solucao solucao_ideia_tags_click chip--active';
                        span5.className = "icon icon--leadind chip--check";
                        elementI2.className = 'fa fa-check'
                        span5.appendChild(elementI2);
                        span6.textContent = data[i].SOLUCAO;
                        div2.appendChild(span5);
                        div2.appendChild(span6);

                    }

                }
                
                Tags.configure();

                $('#texto_pergunta_1').html("");
                $('#texto_pergunta_1').append(div2);

                $('#img_loading_solucoes').attr("style", "display:none;");
            },
            error: function () {
                Messages('error');
            }
        });
}

function CarregarSolucoesModal() {
    let id_ideia = $('#id_ideia').html();
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarSolucoes?idIdeia=' + id_ideia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_solucoes_modal').attr("style", "display:block;");
            },
            success: function (data) {

                // Limpa tags modal 
                var $select = $(document.getElementById('tagsSolucoes'));
                var selectize = $select[0].selectize;
                selectize.clear();

                for (var i = 0; i < data.length; i++) {

                    // Add tags modal                    
                    selectize.addOption({ value: data[i].ID, text: data[i].SOLUCAO });
                    selectize.addItem(data[i].ID);
                    selectize.refreshOptions();                   
                }

                $('#img_loading_solucoes_modal').attr("style", "display:none;");
            },
            error: function () {
                Messages('error');
            }
        });
}

function SalvarCabecalho() {

    let id_ideia = $('#id_ideia').html();
    let nome_ideia = $('#InputTextoIdeia').val();

    if (nome_ideia.trim() == '') {
        Messages('warning', 'ATENÇÃO!', 'Obrigatório informar o nome da ideia!');
        $('#InputTextoIdeia').focus();
        return;
    }

    let ObjIdeia = '{';
    ObjIdeia += '"ID":"' + id_ideia + '",';
    ObjIdeia += '"NOMEIDEIA":"' + nome_ideia + '"';

    ObjIdeia += '}';

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/EditCabecalhoIdeia',
            data: ObjIdeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                
                if (data == 'True') {
                    Messages('success', 'SUCESSO!', 'Ideia cadastrada com sucesso');

                    $('#InputTextoIdeia').val('');

                    CarregarInformacoesCabecalho();
                    $('.btn-fechar-modal').click();
                }
                else {
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao salvar, tente mais tarde.');
                }
            },
            error: function () {
                Messages('error');
            }
        });
}

function SalvarDescricaoProblema() {

    let id_ideia = $('#id_ideia').html();   
    let dor_ideia = $('#InputTextoDor').val();

    let ObjIdeia = '{';
    ObjIdeia += '"ID":' + id_ideia + ',';
    ObjIdeia += '"DOR":"' + dor_ideia + '"';

    ObjIdeia += '}';

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/EditDescricaoProblema',
            data: ObjIdeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {

                if (data == 'True') {
                    Messages('success', 'SUCESSO!', 'Descrição do problema salva com sucesso');

                    $('#InputTextoDor').val('');

                    CarregarDescricaoProblema();
                    $('.btn-fechar-modal').click();
                }
                else {
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao salvar, tente mais tarde.');
                }
            },
            error: function () {
                Messages('error');
            }
        });
}

function SalvarSolucaoProjeto() {

    let id_ideia = $('#id_ideia').html();
    let idTagSolucao = $('.chip--active').attr("data-id-tag");

    let ObjIdeia = '{';
    ObjIdeia += '"ID":"' + idTagSolucao + '",';
    ObjIdeia += '"ID_PROJETO":' + id_ideia + '';

    ObjIdeia += '}';

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/EditSolucaoProjeto',
            data: ObjIdeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {

                if (data == 'False') {
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao salvar a tag no projeto, tente mais tarde.');
                }                
            },
            error: function () {
                Messages('error');
            }
        });
}

function SalvarSolucoes() {

    var listaTags = $('#tagsSolucoes').children();
    let ObjTags = '';

    if (listaTags.length > 0) {

        for (var i = 0; i < listaTags.length; i++) {
            if (listaTags[i].getAttribute('selected') === 'selected') {
                
                ObjTags += '{';
                ObjTags += '"ID":"' + listaTags[i].value + '",';
                ObjTags += '"ID_PROJETO":' + $('#id_ideia').html() + ',';
                ObjTags += '"SOLUCAO":"' + listaTags[i].text + '"';

                if (i + 1 >= listaTags.length) {
                    ObjTags += '}';
                } else {
                    ObjTags += '},';
                }

            }
        }
    } else {
        Messages('warning', 'ATENÇÃO!', 'Necessário informar a tag para conseguir salvar');
        ObjTags = '{}';
        return;
    }

    ObjTags = '[' + ObjTags + ']';
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/SalvarSolucoes',
            data: ObjTags,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {

                if (data == true) {
                    Messages('success', 'SUCESSO!', 'Soluções salvas com sucesso.');

                    CarregarSolucoes();
                    $('.btn-fechar-modal').click();
                }
                else {
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao salvar, tente mais tarde.');
                }

            },
            error: function () {
                Messages('error');
            }
        }
    );

}

function CadastrarRespostaPergunta() {

    let id_questao = $('#id_questao_modal').html();
    let id_ideia = $('#id_ideia_modal').html();
    let email_user = $('#email_user_logado').html();
    let resposta_pergunta = $('#InputTextoRespostaPergunta').val();

    let ObjIdeia = '{';
    ObjIdeia += '"ID_IDEIA":"' + id_ideia + '",';
    ObjIdeia += '"ID_PERGUNTA":"' + id_questao + '",';
    ObjIdeia += '"EMAIL":"' + email_user + '",';
    ObjIdeia += '"RESPOSTA":"' + resposta_pergunta + '"';

    ObjIdeia += '}';

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Ideia/CadastrarRespostaPergunta',
            data: ObjIdeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_pergunta_' + id_questao).attr("style", "display:block;");
            },
            success: function (data) {

                if (data == 'True') {

                    Messages('success', 'SUCESSO!', 'Ideia cadastrada com sucesso');
                    $('#InputTextoRespostaPergunta').val('');
                    $('.btn-fechar-modal').click();
                    $('#resposta_pergunta_' + id_questao).html(resposta_pergunta);

                    $('#img_loading_pergunta_' + id_questao).attr("style", "display:none;");
                } else if (data == 'Máximo de 800 caracteres!')
                {
                    Messages('warning', 'ATENÇÃO!', data);
                    $('#img_loading_pergunta_' + id_questao).attr("style", "display:none;");
                }
                else {                    
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao salvar, tente mais tarde.');
                }

            },
            error: function () {
                Messages('error');
            }
        });
}

function CarregarRespostas() {

    $('.resposta_pergunta').each(function (key, value) {

        let id_ideia = $('#id_ideia').html();
        let id_pergunta = $(this).attr("data-idpergunta");

        $.ajax(
            {
                type: 'GET',
                url: $URL_PRINCIPAL + 'Ideia/BuscarRespostaPergunta',
                data: { idIdeia: id_ideia, idPergunta: id_pergunta},
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {
                    $('#img_loading_pergunta_' + id_pergunta).attr("style", "display:block;");
                },
                success: function (data) {

                    if (data != "") {
                        $('#resposta_pergunta_' + id_pergunta).html(data);
                    } 
                    $('#img_loading_pergunta_' + id_pergunta).attr("style", "display:none;");

                },
                error: function () {
                    Messages('error');
                }
            });

    });
}

function CarregarTodosCardsCoCriacao() {

    $('.cards_colaboracao_item').each(function (key, value) {

        let id_ideia = $('#id_ideia').html();
        let id_pergunta = $(this).attr("data-idpergunta");

        CarregarCadsCoCriacaoPorPergunta(id_ideia, id_pergunta);

    });
}

function CarregarCadsCoCriacaoPorPergunta(id_ideia, id_pergunta) {
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarCardsCoCriacao',
            data: { idIdeia: id_ideia, idPergunta: id_pergunta },
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_pergunta_card_' + id_pergunta).attr("style", "display:block;");
            },
            success: function (data) {

                document.getElementById('cards_colaboracao_item_pergunta_' + id_pergunta).innerHTML = '';
                if (data.length > 0) {

                    /* Status da Ideia */                    
                    let status_ideia = $('#status_ideia').html();

                    /*
                     *   IMG DE LOADING DOS CARDS
                     */
                    let img = document.createElement('img');
                    img.className = "resposta-pergunta-cards-loading";
                    img.setAttribute('data-idpergunta', id_pergunta);
                    img.src = "/images/icon_loading.gif";
                    img.id = "img_loading_pergunta_card_" + id_pergunta;

                    $('#cards_colaboracao_item_pergunta_' + id_pergunta).append(img);
                    
                    for (var i = 0; i < data.length; i++) {

                        /*
                         *   CRIAÇÃO DOS CARDS DE CO-CRIAÇÃO 
                         */

                        let div = document.createElement("div");
                        div.className = 'cards_colaboracao_itens_sem_cursor';
                        div.id = 'cards_colaboracao_itens_' + data[i].ID;       
                        div.setAttribute("style", 'background-color:' + data[i].COR_CARD+';');
                        div.setAttribute("data-idCardCoCriacao", data[i].ID);
                        div.setAttribute("data-idUserCadastrado", data[i].ID_USUARIO);
                        div.setAttribute("data-idPergunta", id_pergunta);
                        div.setAttribute("data-toggle", "modal");

                        if (parseInt(status_ideia) < 2) {
                            div.className = 'cards_colaboracao_itens';
                            div.setAttribute("data-target", "#ModalAddCocriacao");                        
                        }                        

                        let p1 = document.createElement("p");
                        p1.className = "texto_card_colaborar";
                        p1.id = "texto_card_colaborar_" + data[i].ID;
                        p1.textContent = data[i].RESPOSTA;

                        let p2 = document.createElement("p");
                        p2.className = "nome_colaborar";
                        p2.textContent = data[i].NOME_USUARIO;
                        p2.setAttribute("style", "font-size:0.8em; position:relative; bottom: 0;");

                        div.appendChild(p1);
                        div.appendChild(p2);

                        $('#cards_colaboracao_item_pergunta_' + id_pergunta).append(div);

                        $('#cards_colaboracao_item_pergunta_' + id_pergunta).on("click", "#cards_colaboracao_itens_"+data[i].ID, function () {
                            
                            let idCard = $(this).attr("data-idCardCoCriacao");
                            let idUserCadastrado = $(this).attr("data-idusercadastrado");
                            let idPergunta = $(this).attr("data-idPergunta");
                            let descricao = $('#texto_card_colaborar_' + idCard).html();

                            $('.comentario_cocriador').val(descricao);
                            $('#id_card_cocriacao').html(idCard);
                            $('#id_pergunta').html(idPergunta);
                            $('#id_userCadastrado').html(idUserCadastrado);

                        });

                    };
                }

                $('#img_loading_pergunta_card_' + id_pergunta).attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        });

}

function AddCard(idIdeia, id_pergunta, desc) {

    let ObjTotais = '{';
    ObjTotais += '"ID_IDEIA":"' + idIdeia + '",';
    ObjTotais += '"ID_QUESTAO_IDEIA":"' + id_pergunta + '",';
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
                $('#img_loading_pergunta_card_' + id_pergunta).attr("style", "display:block;");
            },
            success: function (data) {
                if (data == 'Máximo de 400 caracteres!') {
                    Messages('warning', 'ATENÇÃO!', data);
                    $('#img_loading_pergunta_card_' + id_pergunta).attr("style", "display:none;");
                    return;
                }

                $('.btn-fechar-modal').click();

                Messages('success', 'SUCESSO!', 'Adicionado com sucesso.');
                CarregarCadsCoCriacaoPorPergunta(idIdeia, id_pergunta);
                $('#img_loading_pergunta_card_' + id_pergunta).attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        });
}

function EditCard(idIdeia, idCard, idPergunta, desc) {
    
    let ObjTotais = '{';
    ObjTotais += '"ID":"' + idCard + '",';
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
                $('#img_loading_pergunta_card_' + idPergunta).attr("style", "display:block;");
            },
            success: function (data) {
                if (data == 'Máximo de 400 caracteres!') {
                    Messages('warning', 'ATENÇÃO!', data);
                    $('#img_loading_pergunta_card_' + idPergunta).attr("style", "display:none;");
                    return;
                }

                Messages('success', 'SUCESSO!', 'Alteração salva com sucesso.');

                CarregarCadsCoCriacaoPorPergunta(idIdeia, idPergunta);
                $('#img_loading_pergunta_card_' + idPergunta).attr("style", "display:none;");
                $('.btn-fechar-modal').click();
            },
            error: function () {
                Messages('error');
            }
        });
}

function DeleteCard(idIdeia, idPergunta, idCard) {
    
    let ObjTotais = '{';
    ObjTotais += '"ID":"' + idCard + '"';
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
                $('#img_loading_pergunta_card_' + idPergunta).attr("style", "display:block;");
            },
            success: function (data) {

                Messages('success', 'SUCESSO!', 'Descrição Excluida com sucesso.');
                CarregarCadsCoCriacaoPorPergunta(idIdeia, idPergunta);
                $('#img_loading_pergunta_card_' + idPergunta).attr("style", "display:none;");
                $('.btn-fechar-modal').click();

            },
            error: function () {
                
                Messages('error');
            }
        });
}

function RecarregarInformacoesCoCriador() {

    let idIdeia = $('#id_ideia').html();
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
                let qntCards = parseInt($('#id_qtd_respostas_cocriadores').html());

                if (parseInt(data) != qntCards) {
                    $('#id_qtd_respostas_cocriadores').html(data);
                    CarregarTodosCardsCoCriacao();
                }

            },
            error: function () {
                Messages('error');
            }
        });
}

function GetHashId(IdIdeia) {
    let hashid = "0";
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/GetHashId?id=' + IdIdeia,
            async: false,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
            },
            success: function (data) {
                hashid = data;
            },
            error: function () {
                hashid = "0";   
            }
        });

    return hashid;
}

function CarregarAnexos() {

    let idIDeia = $('#id_ideia').html();
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/GetAnexos?idIdeia=' + idIDeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#grid_anexos_ideia_conteudo').innerHTML = '';
                $("#grid_anexos_ideia").attr("style", "display:none;");
            },
            success: function (data) {

                if (data.length > 0) {

                    for (var i = 0; i < data.length; i++) {
                        let tr = document.createElement("tr");
                        let td1 = document.createElement("td");
                        let td2 = document.createElement("td");
                        let a = document.createElement("a");

                        td1.textContent = data[i];
                        a.href = $URL_PRINCIPAL + 'Files_PDF/' + data[i] + '.pdf';
                        a.setAttribute("target", "_blanck");
                        a.text = "Baixar";
                        td2.appendChild(a);

                        tr.appendChild(td1);
                        tr.appendChild(td2);

                        document.getElementById("grid_anexos_ideia_conteudo").appendChild(tr);
                        $("#grid_anexos_ideia").attr("style", "display:block;");
                    }
                }


            },
            error: function () {
                Messages('error');
            }
        }
    );
}

function VerificarRespostas() {

    let retorno = true;
    let cont = 0;
    $('.resposta_pergunta').each(function (key, value) {
        cont++;
    });


    let id_ideia = $('#id_ideia').html();
        
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarTotalRespostaPergunta',
            data: { idIdeia: id_ideia },
            async: false,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
            },
            success: function (data) {

                if (parseInt(data) < cont) {
                    retorno = false;
                }
                    
            },
            error: function () {
                Messages('error');
            }
        });


    return retorno;
}