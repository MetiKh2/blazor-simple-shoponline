window.ShowToastr = (type, message) => {
    if (type == 'success') {
        toastr.success(message, 'Opration Successful')
    }
    if (type == 'error') {
        toastr.error(message, 'Opration Failed')
    }
    if (type == 'info') {
        toastr.info(message, 'Information')
    }
    if (type == 'warn') {
        toastr.warning(message, 'Warning')
    }
}