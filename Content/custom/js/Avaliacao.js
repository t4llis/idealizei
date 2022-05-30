$(document).ready(function () {

    let urlAtual = window.location.href;
    if (urlAtual.toUpperCase().includes(($URL_PRINCIPAL + 'avaliacao/ideias/?idIdeia').toUpperCase())) {
        
        CarregarDescricaoProblemaTelaAvaliacao();
        CarregarSolucoesTelaAvaliacao();
        CarregarAnexosAvalicao();

        $('.btn-enviar-avaliacao').on("click", function () {

            
            let idTag = $('.chip_solucao.chip--active').attr("data-id-tag");

            if (idTag == undefined || idTag == null) {
                Messages('warning', 'ATENÇÃO!', 'Obrigatório selecionar uma tag do grupo de avaliadores!');
                return;
            }

            let observacao = $('#observacao').val();
            observacao = observacao.trim();

            if (observacao == '') {
                Messages('warning', 'ATENÇÃO!', 'Obrigatório informar o feedback final!');
                $('#observacao').focus();
                return;
            }

            gravaEnviaAvaliacao(null);
        });
    }     
});

function gravaEnviaAvaliacao(idTag) {
    Loading.start();
    var listaAvaliacao = [];

    let id_ideia = $('#id_ideia').html();

    for (var i = 0; i < avaliacoesIdeias.length; i++) {
        let pontuacao = $("#pontuacao_" + avaliacoesIdeias[i].IDQUESTAOAVALIACAOIDEIA).val();
        let idQuestaoAvaliacaoIdeia = avaliacoesIdeias[i].IDQUESTAOAVALIACAOIDEIA;
        let observacao = $('#observacao').val();

        let ObjAvaliacao = {};
        ObjAvaliacao.PONTUACAO = pontuacao; /*Codigo provisorio, ajustar depois que ajustarmos os cadastros*/
        ObjAvaliacao.IDPROJETO = id_ideia; /*Codigo provisorio, ajustar depois que ajustarmos os cadastros*/
        ObjAvaliacao.IDQUESTAOAVALIACAOIDEIA = idQuestaoAvaliacaoIdeia;/*Codigo provisorio, ajustar depois que ajustarmos os cadastros*/
        ObjAvaliacao.OBSERVACAO = observacao;
        ObjAvaliacao.IDTAGS = idTag;

        listaAvaliacao.push(ObjAvaliacao);            
    }       

    enviaAvaliacao(listaAvaliacao);
} 

function enviaAvaliacao(listaAvaliacao) {       
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Avaliacao/CadastrarAvaliacao',
            data: JSON.stringify(listaAvaliacao),
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                localStorage.clear();
                if (data) {
                    Loading.done();
                    Messages('success', 'SUCESSO!', 'Avaliação cadastrada com sucesso');
                    window.location.href = $URL_PRINCIPAL + 'Avaliacao/';
                }
                else {
                    Loading.done();
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao cadastrar sua Avaliação, tente mais tarde.');
                }
            },
            error: function () {        
                Loading.done();
                Messages('error');
            }
        });
}

function CarregarDescricaoProblemaTelaAvaliacao() {    
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

function CarregarSolucoesTelaAvaliacao() {
    let id_ideia = $('#id_ideia').html();

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

                for (var i = 0; i < data.length; i++) {

                    // Add tags tela principal
                    let div = document.createElement("div");

                    if (data[i].ID == data[i].ID_SOLUCAO_SELECIONADO) {
                        div.className = 'chip_solucao chip--active';
                    } else {
                        div.className = 'chip_solucao';
                    }

                    div.setAttribute("data-id-tag", data[i].ID);
                    div.setAttribute("style", "cursor: default;")
                    div.setAttribute("data-readonly", true);                    

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
                }

                Tags.configure();

                $('#img_loading_solucoes').attr("style", "display:none;");
            },
            error: function () {
                Messages('error');
            }
        });
}

function CarregarAnexosAvalicao() {

    let idIDeia = $('#id_ideia').html();
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/GetAnexos?idIdeia=' + idIDeia,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#grid_anexos_ideia_conteudo_avaliacao').innerHTML = '';
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

                        document.getElementById("grid_anexos_ideia_conteudo_avaliacao").appendChild(tr);
                    }
                }


            },
            error: function () {
                Messages('error');
            }
        }
    );
}
