
var url = (window.location.href).split('/')
if (url[url.length - 3] == 'checkout') {
    localStorage.setItem('check', url[url.length - 1])
}

var DETALHES_PLANO = {}

var monthStr = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro']
var data = new Date();
var dia = String(data.getDate()).padStart(2, '0');
var mes = monthStr[data.getMonth()];
var ano = data.getFullYear();
var dataAtualStr = dia + ' de ' + mes + ' de ' + ano;


function buscarDadosPlano(idPlano) {
    $.ajax({
        type: "POST",
        url: "/Adm/SelectPlano",
        data: {
            ID_PLANO: localStorage.getItem('check')
        },
        dataType: 'json',
        success: function (ret) {
            DETALHES_PLANO = ret
            //console.log(ret)
            if (ret.ID_PLANO > 0) {
                $('#check_desc').val("0,00")
                $("#valorDesconto").text("0,00")
                $('#check_total').val(ret.PLANO_VALORTOTAL)
                $('#check_total_desc').val(ret.PLANO_VALORTOTAL)
                $('#planoTitulo').text(ret.PLANO_TITULO)
                $('#valorPlano').text((parseFloat(ret.PLANO_VALORTOTAL)).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                $('#valorTotal').text((parseFloat(ret.PLANO_VALORTOTAL)).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                $('#planoDescricao').text(ret.PLANO_DESCRICAO)
                $('body').removeClass('load').addClass('body')
            } else {
                localStorage.removeItem('check')
                Swal.fire({
                    position: 'top-center',
                    icon: 'info',
                    title: 'Ops... Nenhum plano foi selecionado!',
                    text: 'Você será redirecionado para que possa escolher um de nossos planos.',
                    showConfirmButton: false,
                    timer: 5000
                })
                window.location.href = "https://idealizei.com.br"
            }            
        },
        error: function (error) {
            console.log('Deu erro: ' + error)
            if (document.referrer != window.location.href) {
                //window.location.href = document.referrer
            }
        },
        complete: function () {
            //$('#formCreateCupom')[0].reset()
            //$('#modal_cadastro_Cupom button[type="submit"]').attr('data-typeCupom', 'create').text('Incluir')
            //bootbox.hideAll();
        }
    });
}

var IDUSUARIO, IDCUPOM
// Verifica se usuário está logado
$.ajax({
    type: "POST",
    url: "/Login/verificaLogin",
    dataType: 'json',
    success: function (ret) {
        IDUSUARIO = ret
        var idPlano = localStorage.getItem('check')
        if ((ret) && (idPlano)) {
            buscarDadosPlano(idPlano)
        } else {
            window.location.href = "/"
        }
    },
    error: function (error) {
        console.log('vish! Access')
    }
});


// Carrega as máscara dos campos
$('#cep').mask('00000-000');
var options = {
    onKeyPress: function (cpf, ev, el, op) {
        var masks = ['000.000.000-000', '00.000.000/0000-00'];
        $('#cpf_cnpj').mask((cpf.length > 14) ? masks[1] : masks[0], op);
    }
}
$('#cpf_cnpj').length > 11 ? $('#cpf_cnpj').mask('00.000.000/0000-00', options) : $('#cpf_cnpj').mask('000.000.000-00#', options);
$('#telefone').mask('(00) 90000-0000');
$('#cartao_numero').mask('0000 0000 0000 0000');
$('#cartao_validade').mask('00/00');
$('#cartao_cvv').mask('0000');

function validaForm(thisInput, ctr = false) {
    var err = 0

    if (thisInput.attr('id') != "complemento" && thisInput.attr('id') != "cupom") {
        if (thisInput.val() == "") {
            thisInput.css({ "border": "1px solid rgb(255,112,112)" })
            err++
        } else {
            if (thisInput.attr('id') === "cpf_cnpj" && thisInput.val().length == 14) {
                if (is_cpf(thisInput.val())) {//Válido
                    $(`label[for="${thisInput.attr('id')}"]`).removeClass('text-danger').text('CPF / CNPJ')
                    thisInput.css({ "border": "1px solid #ced4da" })
                } else {//Válido
                    $(`label[for="${thisInput.attr('id')}"]`).addClass('text-danger').append(' * <strong>Inválido</strong>')
                    thisInput.css({ "border": "1px solid rgb(255,112,112)" })
                    err++
                }
            } else {
                $(`label[for="${thisInput.attr('id')}"]`).removeClass('text-danger')
                thisInput.css({ "border": "1px solid #ced4da" })
            }
        }
    } else {
        if (thisInput.attr('id') === "cupom" && ctr) {
            if (thisInput.val().length > 0) {
                //console.log(`Cupom: ${thisInput.val()}`)
                $.ajax({
                    type: "POST",
                    url: "/Adm/SelectCup",
                    data: {
                        CUPOM: thisInput.val()
                    },
                    dataType: 'json',
                    success: function (ret) {
                        //console.log(ret)
                        IDCUPOM = ret.ID_CUPOM
                        if (ret.ID_CUPOM != 0) {
                            var valDesc = $('#check_total').val() * (ret.DESCONTO / 100)
                            $('#check_total_desc').val($('#check_total').val() - valDesc)
                            $('#check_desc').val(valDesc)
                            $('#check_perc_desc').val(ret.DESCONTO)
                            $('#valorDesconto').text((parseFloat(valDesc)).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                            $('#valorTotal').text((parseFloat($('#check_total').val() - valDesc)).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                        } else {
                            buscarDadosPlano(0)
                            localStorage.removeItem('idPlano')
                            //window.location.href = "https://idealizei.com.br"
                        }
                    },
                    error: function (error) {
                        console.log('Deu erro: ' + error)
                    }
                });
            } else {
                // console.log('cupom vazio')
                buscarDadosPlano(0)
            }
        }
    }
    return err
}

var data = []
// Verifica se o campo atual está vazio
$("input").blur(function () { validaForm($(this), true) });

// Processa dados do usuário
//var dadosArray = []
$(document).on('click', '.method_payment', function (e) {
    e.preventDefault()
    var cont = 0, arr = 0
    //dadosArray.length = 0
    $("#form_dados_pessoais form input").each(function () {

        data[arr] = $(this).val()
        cont += validaForm($(this), false)

        arr++
    })
    if (cont === 0) {

        var methodPayment = $(this).attr('data-payment');
        data[6] = methodPayment

        $('#form_dados_pessoais').hide('slow')
        if (methodPayment === "cc") {
            $('#form_dados_cartao').show('slow')
        } else {
            $('#form_dados_endereco').show('slow')
        }
        //console.log(data)
    }
})

// Processa Cartão de Crédito
//var dadosCcArray = [];
$(document).on('click', '.submit_cart', function (e) {
    e.preventDefault()
    var cont = 0, arr = 7
    //dadosCcArray.length = 0
    $('#form_dados_cartao form input[type="text"]').each(function () {
        data[arr] = $(this).val();
        if ($(this).val() == "") {
            $(this).css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" });
            cont++;
        } else {
            $(this).css({ "border": "1px solid #ced4da", "padding": "2px" });
        }
        arr++
    });
    if (cont == 0) {
        data[arr] = $("#parcelas option:selected").val()
        if ($('#termos_uso').is(':checked') && $('#politica_privacidade').is(':checked')) {
            data[arr+1] = ($('#termos_uso').is(':checked'));
            data[arr+2] = ($('#politica_privacidade').is(':checked'));
            $('#form_dados_cartao').hide('slow')
            $('#form_dados_endereco').show('slow')
            $('#termosMessage').text('').removeClass('mb-2')
        } else {
            $('#termosMessage').text('Aceite os termos para finalizar seu pagamento.').addClass('mb-2')
        }
        //console.log(dadosCcArray)
    }
})
// Processa Endereço
//var dadosEndArray = [];
$(document).on('click', '.submit_finaly', function (e) {
    e.preventDefault()
    var cont = 0, arr = 14
    //dadosEndArray.length = 0
    $("#form_dados_endereco form input").each(function () {
        data[arr] = $(this).val();
        cont += validaForm($(this), true)
        if ($(this).attr('id') != "complemento") {
            if ($(this).val() == "") {
                $(this).css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" });
                cont++;
            } else {
                $(this).css({ "border": "1px solid #ced4da", "padding": "2px" });
            }
        }
        arr++
    });
    if (cont == 0) {

        var apiKey = "ak_live_AC9CioWdIZ5QKLk5Pcx3T1oYa3VPoV", method_payment = data[6], formaPagamento = (method_payment === "boleto") ? 1 : 0,
            statusPayment = (data[6] == "boleto") ? 1 : 2,
            cepValid = (data[18]).replace("-", ""),
            phoneValid = ((((data[3]).replace("-", "")).replace(" ", "")).replace("(", "")).replace(")", "")
        dataPayment = (method_payment === "boleto") ? '"payment_method": "boleto"' : `"card_number": "${(data[7]).replace(" ", "")}", "card_cvv": "${data[10]}", "card_expiration_date": "${(data[9]).replace("/", "")}", "card_holder_name": "${data[8]}", "installments": "${data[11]}"`,
            dados = `{
                "api_key": "${apiKey}",
                "amount": ${data[17]*100},
                ${dataPayment},
                "customer": {
                    "external_id": "#3311",
                    "name": "${data[0]}",
                    "country": "br",
                    "type": "individual",
                    "email": "${data[4]}",
                    "documents": [{"type": "cpf", "number": "${(data[1]).replace(".", "")}" }],
                    "phone_numbers": ["+55${phoneValid}"],
                    "birthday": "${data[2]}"
                },
                "billing": {
                    "name": "${data[0]}",
                    "address": {
                        "country": "br",
                        "state": "${data[24]}",
                        "city": "${data[23]}",
                        "neighborhood": "${data[22]}",
                        "street": "${data[19]}",
                        "street_number": "${data[20]}",
                        "zipcode": "${cepValid}"
                    }
                },
                "items": [
                    {
                        "id": "${data[26]}",
                        "title": "${data[25]}",
                        "unit_price": ${data[17]*100},
                        "quantity": 1,
                        "tangible": true
                    }
                ]
            }`;


        var url = "https://api.pagar.me/1/transactions";
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url);
        xhr.setRequestHeader("content-type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                localStorage.setItem('statusPgM', xhr.status)
                localStorage.setItem('retPgM', xhr.responseText)
            }
        };
        xhr.send(dados);

        var controllPayment = parseInt(localStorage.getItem('statusPgM')),
            retPgM = JSON.parse(localStorage.getItem('retPgM'))
        localStorage.removeItem('statusPgM')
        localStorage.removeItem('retPgM')




        // Se pagamento estiver completo
        if (controllPayment === 200) {
            // salva as informação na tabela carrinho (idUsuario, dataHora, idCupom, totalProdutos, descontoCupom, valorTotal, idFormaPagto, status)
            $.ajax({
                type: "POST",
                url: "/Adm/CreateCarrinho",
                data: {
                    ID_USUARIO: IDUSUARIO,
                    ID_CUPOM: IDCUPOM,
                    TOTAL_PRODUTOS: data[14],
                    DESCONTO_CUPOM: data[15],
                    VALOR_TOTAL: data[17],
                    ID_FORMAPAGTO: formaPagamento,
                    STATUS: statusPayment
                },
                dataType: 'json',
                beforeSend: function () {
                    Swal.fire({
                        position: 'top-center',
                        icon: 'info',
                        title: 'Aguarde... Processando pagamento.',
                        showConfirmButton: false,
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        },
                        timer: 3000
                    })
                },
                success: function (ret) {
                    if (ret) {
                        $.ajax({
                            type: "POST",
                            url: "/Adm/CreateContrato",
                            data: {
                                CONTRATO_IDUSUARIO: IDUSUARIO,
                                CONTRATO_IDPLANO: DETALHES_PLANO.ID_PLANO,
                                CONTRATO_DATARENOVACAO: '',
                                CONTRATO_DATACANCELAMENTO: '',
                                CONTRATO_QUANTUSUARIOS: DETALHES_PLANO.PLANO_QUANTUSUARIOS,
                                CONTRATO_QUANTIDEIAS: DETALHES_PLANO.PLANO_QUANTIDEIAS,
                                CONTRATO_QUANTVALIDACOES: DETALHES_PLANO.PLANO_QUANTVALIDACOES,
                                CONTRATO_VALORUSUARIO: (DETALHES_PLANO.PLANO_VALORUSUARIO).replace(".", ","),
                                CONTRATO_VALORIDEIA: (DETALHES_PLANO.PLANO_VALORIDEIA).replace(".", ","),
                                CONTRATO_VALORVALIDACAO: (DETALHES_PLANO.PLANO_VALORVALIDACAO).replace(".", ","),
                                CONTRATO_VALORTOTAL: (data[17]).replace(".", ","),
                                CONTRATO_STATUS: 1
                            },
                            dataType: 'json',
                            success: function (ret) {
                                console.log('ok')
                                localStorage.removeItem('check')
                                localStorage.removeItem('statusPgM')
                                localStorage.removeItem('retPgM')
                            },
                            error: function (error) {
                                console.log(error)
                            }
                        });
                    }
                    if (data[6] == "boleto") {
                        $('#option_payment_boleto').css({'display': 'block'})
                        $('#option_payment_boleto a').attr('href', retPgM.boleto_url+'?format=pdf')
                    }

                    var mtdPay = (data[6] == 'boleto') ? 'Boleto' : 'Cartão de Crédito'

                    $('#info_form_numero_pedido').text(retPgM.id)
                    $('#info_form_data').text(dataAtualStr)
                    $('#info_form_email').text(data[4])
                    $('#info_form_total').text((parseFloat(data[17])).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                    $('#info_form_metodo_pagamento').text(mtdPay)

                    $('#info_form_nome_plano').text(DETALHES_PLANO.PLANO_TITULO)
                    $('#info_form_valor').text((parseFloat(data[14])).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                    $('#info_form_subtotal').text((parseFloat(data[14])).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                    $('#info_form_desconto').text((parseFloat(data[15])).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))
                    $('#info_form_payment').text(mtdPay)
                    $('#info_form_total2').text((parseFloat(data[17])).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }))

                    $('#info_form_nome').text(data[0])
                    $('#info_form_endereco').text(data[19]+', '+data[20])
                    $('#info_form_bairro').text(data[22])
                    $('#info_form_cidade').text(data[23])
                    $('#info_form_estado').text(data[24])
                    $('#info_form_cep').text(data[18])
                    $('#info_form_telefone').text(data[3])
                    $('#info_form_email2').text(data[4])


                    $('#form_dados_endereco').hide('slow')
                    $('#resumo_compra').css({ 'display': 'block' })
                    
                    
                },
                error: function (error) {
                    console.log('vish! Access')
                }
            });
        } else {
            Swal.fire({
                position: 'top-center',
                icon: 'error',
                title: 'Erro ao processar pagamento.',
                text: 'Verifique se todos os dados estão preenchidos corretamente',
                showConfirmButton: false,
                timer: 5000
            })
        }

    }
})

$('.prevForm').on('click', function(ee) {
    ee.preventDefault()

    if (data[6] === 'boleto') {
        $(`#form_dados_pessoais`).show('slow')
    } else {
        $(`#${$(this).attr('data-prev')}`).show('slow')
    }

    $(`#${$(this).attr('data-atual')}`).hide('slow')
})


// Valida CPF
function is_cpf(c) {

    if ((c = c.replace(/[^\d]/g, "")).length != 11)
        return false

    if (c == "00000000000")
        return false;

    var r;
    var s = 0;
    for (i = 1; i <= 9; i++)
        s = s + parseInt(c[i - 1]) * (11 - i);

    r = (s * 10) % 11;
    if ((r == 10) || (r == 11))
        r = 0;

    if (r != parseInt(c[9]))
        return false;

    s = 0;
    for (i = 1; i <= 10; i++)
        s = s + parseInt(c[i - 1]) * (12 - i);

    r = (s * 10) % 11;
    if ((r == 10) || (r == 11))
        r = 0;

    if (r != parseInt(c[10]))
        return false;

    return true;
}

// Preeche dados através do cep
$(document).ready(function () {
    $('#form_dados_cartao').hide();
    $('#form_dados_endereco').hide();
    function limpa_formulario_cep() {
        $("#endereco").val("");
        $("#bairro").val("");
        $("#cidade").val("");
        $("#estado").val("");
    }
    $("#cep").blur(function () {
        $("#cepMessage").text("");
        var cep = $(this).val().replace(/\D/g, '');
        if (cep != "") {
            var validacep = /^[0-9]{8}$/;
            if (validacep.test(cep)) {
                $("#endereco").val("...");
                $("#bairro").val("...");
                $("#cidade").val("...");
                $("#estado").val("...");
                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
                    if (!("erro" in dados)) {
                        $("#endereco").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $("#cidade").val(dados.localidade);
                        $("#estado").val(dados.uf);
                    } else {
                        limpa_formulario_cep();
                        $("#cepMessage").text("CEP não encontrado.");
                    }
                });
            } else {
                limpa_formulario_cep();
                $("#cepMessage").text("Formato de CEP inválido.");
            }
        } else {
            limpa_formulario_cep();
        }
    });
});

(function ($) {
    $(function () {
        //$('body').removeClass('load').addClass('body')
    });
})(jQuery);