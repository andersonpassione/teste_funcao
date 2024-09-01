function formatarCpf(input) {
    var cpf = input.value;
    cpf = cpf.replace(/\D+/g, '');
    cpf = cpf.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})$/, '$1.$2.$3-$4');    
    input.value = cpf;
}