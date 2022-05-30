$('#pills-tab li a').on('click', function () {
    localStorage.setItem('paramPage', $(this).attr('id'))
})
var param = localStorage.getItem('paramPage')
if (param) {
    $(`#${param}`).click();
}


// Forms
function novoCadastro(nameForm, actionForm) {
    var modal_cadastro = $(`#modal_cadastro_${nameForm}`);
    $('[type="date"]').attr('value', '')
    var daType = (actionForm == "Alterar") ? 'update' : 'create'
    var daTxt = (actionForm == "Alterar") ? 'Alterar' : 'Incluir'
    $('button[type="submit"]').attr(`data-type${nameForm}`, daType).text(daTxt)
    $(`#formCreate${nameForm}`)[0].reset()
    var today = new Date()
    var dateNowMonth = String(today.getMonth() + 1).padStart(2, '0')
    var dateNowDay = String(today.getDate()).padStart(2, '0')
    var dateNowString = `${today.getFullYear()}-${dateNowMonth}-${dateNowDay}`
    $(`#contrato_data_cadastro`).val(dateNowString)

    bootbox.dialog({
        title: `${actionForm} ${nameForm}`,
        message: modal_cadastro
    })
    .on('shown.bs.modal', function () {
        modal_cadastro.show(0, function () {
            $('form').focus()
        }).removeClass('d-none')
    })
    .on('hidden.bs.modal', function () {
        modal_cadastro.hide().appendTo('body')
    })
    $('.modal-dialog').addClass('modal-xl')

}
$(document).on('click', '.novoCadastro', function () {
    var nameForm = $(this).attr('data-form')
    var actionForm = $(this).attr("data-formAction")
    novoCadastro(nameForm, actionForm)
})

// Formata data para incluir no HTML(input)
function formatDate(date) {
    var d = date.split('/')
    return `${d[2]}-${d[1]}-${d[0]}`
}


function verificaMaior(qtd, qtd2) {
    if ( (qtd >= qtd2) && (qtd > 0) && (qtd2 > 0) ) {
        $("#quantidade").css({ "border": "1px solid #ced4da", "padding": "2px" });
        $("#cupom_quantidade_disponivel").css({ "border": "1px solid #ced4da", "padding": "2px" });

        console.log("ok")
        return "one"
    } else {
        $("#quantidade").css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" });
        $("#cupom_quantidade_disponivel").css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" });
        console.log("menor")
        return "two"
    }
}

$("#cupom_quantidade_disponivel").on('blur', function () {
    verificaMaior($("#quantidade").val(), $("#cupom_quantidade_disponivel").val())
})
$("#quantidade").on('blur', function () {
    verificaMaior($("#quantidade").val(), $("#cupom_quantidade_disponivel").val())
})

// Cupom
$('#formCreateCupom').on('submit', function (e) {
    e.preventDefault();

    var cont = 0;
    $("#formCreateCupom input").each(function () {
        

        if (($(this).attr('id') === "cupom_quantidade_disponivel") || ($(this).attr('id') === "quantidade")) {
            if (verificaMaior($("#quantidade").val(), $("#cupom_quantidade_disponivel").val()) == "one") {

            } else {
                cont++
            }
        } else {
            if ($(this).val() == "") {
                $(this).css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" });
                cont++;
            } else {
                $(this).css({ "border": "1px solid #ced4da", "padding": "2px" });
            }
        }
    });
    if (cont == 0) {
        var url = "/Adm/CreateCupom"
        if ($('#formCreateCupom button[type="submit"]').attr('data-typeCupom') == "update") {
            url = "/Adm/AlterarCupom"
        }
        $.ajax({
            type: "POST",
            url: url,
            data: $(this).serialize(),
            dataType: 'json',
            beforeSend: function () {
                $('#modal_cadastro_Cupom button[type="submit"]').text('Processando...')
            },
            success: function (ret) {
                window.location.href = ""
            },
            error: function (error) {
                console.log(error)
            },
            complete: function () {
                $('#formCreateCupom')[0].reset()
                $('#modal_cadastro_Cupom button[type="submit"]').attr('data-typeCupom', 'create').text('Incluir')
                bootbox.hideAll();
            }
        });
    }
})
$('.alterarCupom').on('click', function () {
    var idCupomAlter = $(this).attr('data-cIdCupomAlter')
    //console.log(idCupomAlter)

    $.ajax({
        type: "POST",
        url: "/Adm/SelectCupom",
        data: {
            ID_CUPOM: idCupomAlter
        },
        dataType: 'json',
        success: function (ret) {

            console.log(ret)
            let status = (ret.STATUS != "checked") ? "0" : "1"
            novoCadastro('Cupom', 'Alterar')
            $("#id_cupom").val(ret.ID_CUPOM)
            $("#cupom_codigo").val(ret.CUPOM)
            $("#cupom_descricao").val(ret.DESCRICAO)
            $("#cupom_validade").attr('value', formatDate(ret.VALIDADE))
            $("#desconto").val(ret.DESCONTO)
            $("#quantidade").val(ret.QUANTIDADE)
            $("#cupom_quantidade_disponivel").val(ret.DISPONIVEL)
            $("#status_cupom").val(status)
        },
        error: function (error) {
            console.log(error)
        }
    });
})
$('.deletarCupom').on('click', function () {
    var idCupomDelete = $(this).attr('data-cIdCupom')
    var MessageDelete = `<strong>Id: </strong> ${idCupomDelete}<br>` +
        `<strong>Cupom: </strong> ${$(this).attr("data-cTitle")}<br>` +
        `<strong>Descrição: </strong> ${$(this).attr("data-cDescricao")}<br>` +
        `<strong>Validade: </strong> ${$(this).attr("data-cValidade")}<br>` +
        `<strong>Desconto: </strong> ${$(this).attr("data-cDesconto")}%<br>` +
        `<strong>Status: </strong> ${$(this).attr("data-cStatus")}<br>`

    bootbox.confirm({
        message: "<h5>Deseja realmente excluir este cupom?</h5><br/>" + MessageDelete,
        buttons: {
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            },
            confirm: {
                label: 'Sim',
                className: 'btn-outline-success'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Adm/DeleteCupom",
                    data: {
                        ID_CUPOM: idCupomDelete
                    },
                    dataType: 'json',
                    beforeSend: function () {
                        console.log('Carregando')
                    },
                    success: function (ret) {
                        window.location.href = ""
                    },
                    error: function (error) {
                        console.log(error)
                    },
                    complete: function () {
                        bootbox.hideAll();
                    }
                });
            }
        }
    });
})









// Contrato 
$('#formCreateContrato').on('submit', function (e) {
    e.preventDefault()

    var cont = 0

    if ($('#formCreateContrato button[type="submit"]').attr('data-typeContrato') == "create") {
        $("#formCreateContrato input").each(function () {
            console.log($(this).attr("type") + ' -- ' + $(this).val())

            if (($(this).attr("id") != "contrato_data_renovacao") && ($(this).attr("id") != "contrato_data_cancelamento")) {
                if ($(this).val() == "") {
                    $(this).css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" })
                    cont++;
                } else {
                    $(this).css({ "border": "1px solid #ced4da", "padding": "2px" })
                }
            }
            /*if ($('#contrato_idUsuario').val() <= 0) {
                $('#contrato_cliente').css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" })
                cont++;
            } else {
                $('#contrato_cliente').css({ "border": "1px solid #ced4da", "padding": "2px" })
            }*/
        });

        if ($("#formCreateContrato select").is(':selected')) {
            $("#formCreateContrato select").css({ "border": "1px solid rgb(255,112,112)" })
        } else {
            $("#formCreateContrato select").css({ "border": "1px solid #ced4da" })
        }
    }

    /*console.log($(this).serialize())

    console.log(cont)*/

    /*CONTRATO_IDUSUARIO = 152 &
    CONTRATO_NOMEUSUARIO=Ezequiel + Teste &
    CONTRATO_STATUS=1 &
    CONTRATO_DATARENOVACAO=2021 - 11 - 30 &
    CONTRATO_DATACANCELAMENTO=2021 - 12 - 01 &
    CONTRATO_TITULOPLANO=19 &
    CONTRATO_IDPLANO=19 &
    CONTRATO_QUANTUSUARIOS=1 &
    CONTRATO_QUANTIDEIAS=1 &
    CONTRATO_QUANTVALIDACOES=1 &
    CONTRATO_VALORUSUARIO=1.99 &
    CONTRATO_VALORIDEIA=1.99 &
    CONTRATO_VALORVALIDACAO=1.99 &
    CONTRATO_VALORTOTAL=5.97 &
    CONTRATO_STATUS=1*/
     
    if (cont == 0) {
        console.log($(this).serialize())

        var url = "/Adm/CreateContrato"
        if ($('#formCreateContrato button[type="submit"]').attr('data-typeContrato') == "update") {
            url = "/Adm/AlterarContrato"
        }
        
        $.ajax({
            type: "POST",
            url: url,
            data: $(this).serialize(),
            dataType: 'json',
            success: function (ret) {
                window.location.href = ""
            },
            error: function (error) {
                console.log(error)
            },
            complete: function () {
                $('#formCreateContrato')[0].reset()
                bootbox.hideAll()
            }
        });
    }
})
$('.alterarContrato').on('click', function () {
    var idContratoAlter = $(this).attr('data-cIdContratoAlter')

    $.ajax({
        type: "POST",
        url: "/Adm/SelectContrato",
        data: {
            ID_CONTRATO: idContratoAlter
        },
        dataType: 'json',
        success: function (ret) {
            //console.log(ret)
            let status = (ret.CONTRATO_STATUS != "checked") ? "0" : "1"
            novoCadastro('Contrato', 'Alterar')
            $("#contrato_cliente").val(ret.CONTRATO_NOMEUSUARIO)
            $("#contrato_status option").removeAttr("selected")
            $("#contrato_status option[value='" +status+"']").removeAttr("selected")
            $("#contrato_numero").val(ret.CONTRATO_IDCONTRATO)
            $("#contrato_idUsuario").val(ret.CONTRATO_IDUSUARIO)
            $("#contrato_plano").val(ret.CONTRATO_IDPLANO)
            $("#contrato_idPlano").val(ret.CONTRATO_IDPLANO)
            $("#contrato_data_cancelamento").attr('value', formatDate(ret.CONTRATO_DATACANCELAMENTO))
            $("#contrato_data_cadastro").attr('value', formatDate(ret.CONTRATO_DATACONTRATO))
            $("#contrato_data_renovacao").attr('value', formatDate(ret.CONTRATO_DATARENOVACAO))
            $(".contrato_quantidade_ideias").val(ret.CONTRATO_QUANTIDEIAS)
            $(".contrato_quantidade_usuarios").val(ret.CONTRATO_QUANTUSUARIOS)
            $(".contrato_quantidade-avaliacoes").val(ret.CONTRATO_QUANTVALIDACOES)
            $(".contrato_valor_usuarios").val(ret.CONTRATO_VALORUSUARIO)
            $(".contrato_valor_ideias").val(ret.CONTRATO_VALORIDEIA)
            $(".contrato_valor_avaliacoes").val(ret.CONTRATO_VALORVALIDACAO)
            $(".contrato_valor_total").val(ret.CONTRATO_VALORTOTAL)
            $("#contrato_status").val(ret.CONTRATO_STATUS)
        },
        error: function (error) {
            console.log(error)
        }
    });
})

$('#contrato_cliente').on('keyup', function () {
    console.log($(this).val())
    $.ajax({
        type: "POST",
        url: "/Adm/SelectUserContrato",
        data: {
            NOME: $(this).val()
        },
        dataType: 'json',
        success: function (ret) {
            var lista = ""
            for (var u = 0; u < ret.length; u++) {
                lista += `<option data-value='${ret[u].ID}' value='${ret[u].NOME}'></option>`
            }
            $('#clienteInfos').html(lista)
        },
        error: function (error) {
            console.log(error)
        }
    });
})

$('#contrato_cliente').on('blur', function () {
    $('#contrato_idUsuario').val('')
    var nomeCliente = $(this).val();

    $("#clienteInfos").find("option").each(function () {
        if ($(this).val() == nomeCliente) {
            $('#contrato_idUsuario').val($(this).attr('data-value'));
        }
    })
})

// Busca dados para preencher plano no contrato
$('#contrato_plano').on('change', function () {
    $.ajax({
        type: "POST",
        url: "/Adm/SelectPlano",
        data: {
            ID_PLANO: $(this).val()
        },
        dataType: 'json',
        success: function (ret) {
            //console.log(ret)
            $('.contrato_quantidade_usuarios').val(ret.PLANO_QUANTUSUARIOS)
            $('.contrato_quantidade_ideias').attr('value', ret.PLANO_QUANTIDEIAS)
            $('.contrato_quantidade-avaliacoes').attr('value', ret.PLANO_QUANTVALIDACOES)
            $('.contrato_valor_usuarios-format').attr('value', (ret.PLANO_VALORUSUARIO).replace(".", ","))
            $('.contrato_valor_usuarios').attr('value', ret.PLANO_VALORUSUARIO)
            $('.contrato_valor_ideias-format').attr('value', (ret.PLANO_VALORIDEIA).replace(".", ","))
            $('.contrato_valor_ideias').attr('value', ret.PLANO_VALORIDEIA)
            $('.contrato_valor_avaliacoes-format').attr('value', (ret.PLANO_VALORVALIDACAO).replace('.', ','))
            $('.contrato_valor_avaliacoes').attr('value', ret.PLANO_VALORVALIDACAO)
            $('.contrato_valor_total').val((ret.PLANO_VALORTOTAL).replace(".", ","))
            $('#contrato_idPlano').val(ret.ID_PLANO)
            //window.location.href = ""
        },
        error: function (error) {
            console.log(error)
        }
    });
})

$('.deletarContrato').on('click', function () {
    var idContratoDelete = $(this).attr('data-cIdContrato')
    var MessageDelete = `<strong>Id: </strong> ${idContratoDelete}<br>` +
        `<strong>Usuário: </strong> ${$(this).attr("data-cContratoUsuario")}<br>` +
        `<strong>Contrato Data: </strong> ${$(this).attr("data-cContratoData")}<br>` +
        `<strong>Contrato Renovação: </strong>${$(this).attr("data-cContratoRenovacao")}<br>` +
        `<strong>Contrato Cancelamento: </strong> ${$(this).attr("data-cContratoCancelamento")}<br>` +
        `<strong>Contrato Valor Total: </strong>R$ ${$(this).attr("data-cContratoValTotal")}<br>` +
        `<strong>Status: </strong> ${$(this).attr("data-cContratoStatus")}<br>`

    bootbox.confirm({
        message: "<h5>Deseja realmente excluir este contrato?</h5><br/>" + MessageDelete,
        buttons: {
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            },
            confirm: {
                label: 'Sim',
                className: 'btn-outline-success'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Adm/DeleteContrato",
                    data: {
                        ID_CONTRATO: idContratoDelete
                    },
                    dataType: 'json',
                    success: function (ret) {
                        window.location.href = ""
                    },
                    error: function (error) {
                        console.log(error)
                    },
                    complete: function () {
                        bootbox.hideAll();
                    }
                });
            }
        }
    });
})
// End Contrato









// Plano
$('.alterarPlano').on('click', function () {
    var idPlanoAlter = $(this).attr('data-cIdPlanoAlter')

    $.ajax({
        type: "POST",
        url: "/Adm/SelectPlano",
        data: {
            ID_PLANO: idPlanoAlter
        },
        dataType: 'json',
        success: function (ret) {
            console.log(ret)
            let status = (ret.PLANO_STATUS != "checked") ? "0" : "1"
            
            novoCadastro('Plano', 'Alterar')
            $("#id_plano").val(ret.ID_PLANO)
            $("#plano_nome").val(ret.PLANO_TITULO)
            $("#plano_status option").removeAttr('selected')
            $("#plano_status option[value='"+status+"']").attr('selected')
            //$("#cupom_validade").attr('value', formatDate(ret.VALIDADE))
            $("#plano_quantidade_usuarios").val(ret.PLANO_QUANTUSUARIOS)
            $("#plano_quantidade_ideias").val(ret.PLANO_QUANTIDEIAS)
            $("#plano_quantidade_avaliacoes").val(ret.PLANO_QUANTVALIDACOES)
            $("#plano_valor_unitario_usuarios").val(ret.PLANO_VALORUSUARIO)
            $("#plano_valor_unitario_ideias").val(ret.PLANO_VALORIDEIA)
            $("#plano_valor_unitario_avaliacoes").val(ret.PLANO_VALORVALIDACAO)
            $("#plano_descricao").val(ret.PLANO_DESCRICAO)
            $(".plano_valorTotal").val(ret.PLANO_VALORTOTAL)
        },
        error: function (error) {
            console.log(error)
        }
    });
})
$('#formCreatePlano').on('submit', function (e) {
    e.preventDefault()

    var cont = 0
    if ($('#plano_descricao').val() == "") {
        $('#plano_descricao').css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" })
        cont++
    } else {
        $('#plano_descricao').css({ "border": "1px solid #ced4da", "padding": "2px" })
    }
    $("#formCreatePlano input").each(function () {
        if ($(this).val() == "") {
            $(this).css({ "border": "1px solid rgb(255,112,112)", "padding": "2px" })
            cont++
        } else {
            $(this).css({ "border": "1px solid #ced4da", "padding": "2px" })
        }
    });

    //console.log(cont)
    //console.log($(this).serialize())
    if (cont == 0) {
        var url = "/Adm/CreatePlano"
        if ($('#formCreatePlano button[type="submit"]').attr('data-typePlano') == "update") {
            url = "/Adm/AlterarPlano"
        }

        
        $.ajax({
            type: "POST",
            url: url,
            data: $(this).serialize(),
            dataType: 'json',
            success: function (ret) {
                console.log(ret)
                window.location.href = ""
            },
            error: function (error) {
                console.log(error)
            },
            complete: function () {
                $('#formCreatePlano')[0].reset()
                bootbox.hideAll()
            }
        });
    }      
})
function calValTotPlan() {
    var qtdUserPlan = ($('#plano_quantidade_usuarios').val() > 0) ? $('#plano_quantidade_usuarios').val() : 0
    var qtdIdeiaPlan = ($('#plano_quantidade_ideias').val() > 0) ? $('#plano_quantidade_ideias').val() : 0
    var qtdAvPlan = ($('#plano_quantidade_avaliacoes').val() > 0) ? $('#plano_quantidade_avaliacoes').val() : 0
    var valUserPlan = (parseFloat($('#plano_valor_unitario_usuarios').val()) > 0) ? parseFloat($('#plano_valor_unitario_usuarios').val()) : 0
    var valIdeiaPlan = (parseFloat($('#plano_valor_unitario_ideias').val()) > 0) ? parseFloat($('#plano_valor_unitario_ideias').val()) : 0
    var valAvPlan = (parseFloat($('#plano_valor_unitario_avaliacoes').val()) > 0) ? parseFloat($('#plano_valor_unitario_avaliacoes').val()) : 0
    $('.plano_valorTotal').val((qtdUserPlan * valUserPlan * 1) + (qtdIdeiaPlan * valIdeiaPlan * 1) + (qtdAvPlan*valAvPlan*1))
}
$('.calcValPlan').blur(function () { calValTotPlan() })
$('.deletarPlano').on('click', function () {
    var idPlanoDelete = $(this).attr('data-cIdPlano')
    var MessageDelete = `<strong>Id: </strong> ${idPlanoDelete}<br>` +
        `<strong>Plano: </strong> ${$(this).attr("data-cPlanoTitulo")}<br>` +
        `<strong>Descrição: </strong> ${$(this).attr("data-cPlanoDescricao")}<br>` +
        `<strong>Valor Ideia: </strong>R$ ${$(this).attr("data-cPlanoValIdeia")}<br>` +
        `<strong>Valor Usuário: </strong>R$ ${$(this).attr("data-cPlanoValUsuario")}<br>` +
        `<strong>Valor Validação: </strong>R$ ${$(this).attr("data-cPlanoValValidacao")}<br>` +
        `<strong>Status: </strong> ${$(this).attr("data-cPlanoStatus")}<br>`

    bootbox.confirm({
        message: "<h5>Deseja realmente excluir este plano?</h5><br/>" + MessageDelete,
        buttons: {
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            },
            confirm: {
                label: 'Sim',
                className: 'btn-outline-success'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Adm/DeletePlano",
                    data: {
                        ID_PLANO: idPlanoDelete
                    },
                    dataType: 'json',
                    success: function (ret) {
                        window.location.href = ""
                    },
                    error: function (error) {
                        console.log(error)
                    },
                    complete: function () {
                        bootbox.hideAll();
                    }
                });
            }
        }
    });
})
// End Plano

//$('table').each(function () {
    //$('th').each(function () {
        /*if ($('th').attr('data-width') == 'auto') {
            $('th').css({ 'width': $('th').attr('data-width') })
        } else {
            $('th').css({ 'width': $('th').attr('data-width')+'px' })
        }*/
    //})
//})
$(window).on("load", function () {
    $('th.tam_auto').attr('style', 'width: auto!important')
    $('th.tam_40').attr('style', 'width: 40px!important')
    $('th.tam_50').attr('style', 'width: 50px!important')
    $('th.tam_70').attr('style', 'width: 70px!important')
    $('th.tam_76').attr('style', 'width: 76px!important')
    $('th.tam_80').attr('style', 'width: 80px!important')
    $('th.tam_90').attr('style', 'width: 90px!important')
    $('th.tam_110').attr('style', 'width: 110px!important')
    $('th.tam_120').attr('style', 'width: 120px!important')
    $('th.tam_150').attr('style', 'width: 150px!important')
    $('th.tam_170').attr('style', 'width: 170px!important')
})