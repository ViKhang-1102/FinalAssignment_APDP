window.initStudentSelect2 = function() {
    console.log('initStudentSelect2 called');
    var element = $('#studentIds');
    
    if (element.length === 0) {
        console.log('Element not found');
        return false;
    }
    
    if (element.hasClass('select2-hidden-accessible')) {
        console.log('Destroying existing Select2');
        element.select2('destroy');
    }
    
    element.select2({
        ajax: {
            url: '/api/students/search',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term || '',
                    page: params.page || 1
                };
            },
            processResults: function (data) {
                console.log('Students loaded:', data.length);
                return {
                    results: data.map(function(item) {
                        return {
                            id: item.id,
                            text: item.name + ' (' + item.studentId + ') - ' + item.email
                        };
                    })
                };
            },
            cache: true
        },
        placeholder: 'Search students...',
        minimumInputLength: 0,
        width: '100%'
    });
    
    console.log('Select2 initialized');
    return true;
};

window.clearStudentSelect2 = function() {
    var element = $('#studentIds');
    if (element.length > 0) {
        element.val(null).trigger('change');
    }
};

window.setSelectedStudents = function(students) {
    var element = $('#studentIds');
    if (element.length === 0) return false;

    // remove any existing options to avoid duplicates
    element.find('option').remove();

    if (!students || students.length === 0) {
        element.val(null).trigger('change');
        return true;
    }

    students.forEach(function(s) {
        var option = new Option(s.text, s.id, true, true);
        element.append(option);
    });

    element.trigger('change');
    return true;
};

window.getSelectedStudents = function() {
    var val = $('#studentIds').val();
    return val ? (Array.isArray(val) ? val : [val]) : [];
};
