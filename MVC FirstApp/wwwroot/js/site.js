// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function confirmDelete(isTrue) {
    var deleteSpan = 'deleteSpan';
    var confirmDeleteSpan = 'confirmDeleteSpan';

    if (isTrue) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

function showGroupRenameForm(id) {
    var groupRenameForm_ = 'groupRenameForm_';
    var renameSpan_ = 'renameSpan_';

    $('#' + groupRenameForm_ + id).show();
    $('#' + renameSpan_ + id).hide();
}