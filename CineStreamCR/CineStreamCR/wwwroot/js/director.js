(() => {

    const Director = {
        tabla: null,
        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },
        inicializarTabla() {
            this.tabla = $('#tblDirector').DataTable({

                ajax: {
                    url: '/Director/GetDirectors',
                    type: 'GET',
                    dataSrc: 'dato'
                },
                columns: [
                    { data: 'directorId' },
                    {
                        data: null,
                        render: function (data) {
                            return `${data.firstName} ${data.lastName}`;
                        }
                    },
                    { data: 'biography' },
                    { data: 'nationality' },
                    { data: 'birthDate' },
                    { data: 'pictureImg' },
                    { data: 'isActive' },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        render: (data, type, row) => {
                            return `
                                   <button class="btn btn-sm btn-primary editar" data-id="${row.directorId}">Editar</button>
                                   <button class="btn btn-sm btn-danger eliminar" data-id="${row.directorId}">Eliminar</button>
                                    `
                        }
                    }
                ],

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }

            });
        },
        registrarEventos() {

            $('#btnGuardarDirector').on('click', function () {
                Director.guardarDirector();
            });

            $('#btnEditarDirector').on('click', function () {
                Director.editarDirector();
            });

            $('#tblDirector').on('click', '.editar', function () {
                const id = $(this).data('id');
                Director.cargarDirector(id);
            });

            $('#tblDirector').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Director.eliminarDirector(id);
            });

        },
        guardarDirector() {
            let form = $('#formCrearDirector');

            if (!form.valid()) {
                return;
            }

            // Usamos FormData para poder enviar el archivo (pictureFile)
            // junto con los demás campos. form.serialize() NO envía archivos.
            let formData = new FormData(form[0]);

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: formData,
                processData: false,   // evita que jQuery intente convertir FormData a string
                contentType: false,   // deja que el navegador arme el boundary multipart correcto
                success: function (respuesta) {

                    if (respuesta.esCorrecto) {

                        $('#modalCrearDirector').modal('hide');
                        form[0].reset();
                        Director.tabla.ajax.reload();

                        Swal.fire({
                            title: 'Correcto',
                            text: respuesta.mensaje,
                            icon: 'success'
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Incorrecto',
                            text: respuesta.mensaje,
                            icon: 'error'
                        });
                    }

                },
                error: function (xhr) {
                    // Antes no había manejo de error: si el servidor respondía
                    // 400/500, no pasaba absolutamente nada en pantalla.
                    let mensaje = 'Ocurrió un error al guardar el director.';

                    if (xhr.responseJSON && xhr.responseJSON.mensaje) {
                        mensaje = xhr.responseJSON.mensaje;
                    } else if (xhr.responseJSON && xhr.responseJSON.errors) {
                        // errores de ModelState (400 BadRequest)
                        const errores = Object.values(xhr.responseJSON.errors).flat();
                        mensaje = errores.join('\n');
                    }

                    Swal.fire({
                        title: 'Error',
                        text: mensaje,
                        icon: 'error'
                    });
                }
            })
        },


        editarDirector() {
            let form = $('#formEditarDirector');

            if (!form.valid()) {
                return;
            }

            // Mismo problema y misma solución que en guardarDirector():
            // el formulario tiene enctype multipart/form-data (foto opcional),
            // así que hay que usar FormData en vez de serialize().
            let formData = new FormData(form[0]);

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (respuesta) {

                    if (respuesta.esCorrecto) {

                        $('#modalEditarDirector').modal('hide');
                        form[0].reset();
                        Director.tabla.ajax.reload();

                        Swal.fire({
                            title: 'Correcto',
                            text: respuesta.mensaje,
                            icon: 'success'
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Incorrecto',
                            text: respuesta.mensaje,
                            icon: 'error'
                        });
                    }

                },
                error: function (xhr) {
                    let mensaje = 'Ocurrió un error al actualizar el director.';

                    if (xhr.responseJSON && xhr.responseJSON.mensaje) {
                        mensaje = xhr.responseJSON.mensaje;
                    } else if (xhr.responseJSON && xhr.responseJSON.errors) {
                        const errores = Object.values(xhr.responseJSON.errors).flat();
                        mensaje = errores.join('\n');
                    }

                    Swal.fire({
                        title: 'Error',
                        text: mensaje,
                        icon: 'error'
                    });
                }
            })
        },

        eliminarDirector(id) {

            Swal.fire({
                title: "Estas seguro?",
                text: "No podras revertir esta operacion!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, eliminar",
                cancelButtonText: 'Cancelar'
            }).then((result) => {

                if (result.isConfirmed) {

                    $.ajax({
                        url: `/Director/DeleteDirector?id=${id}`,
                        type: 'POST',
                        success: function (respuesta) {
                            if (respuesta.esCorrecto) {
                                Director.tabla.ajax.reload();
                                Swal.fire({
                                    title: 'Correcto',
                                    text: respuesta.mensaje || 'Director eliminado correctamente',
                                    icon: 'success'
                                });
                            } else {
                                Swal.fire({
                                    title: 'Incorrecto',
                                    text: respuesta.mensaje || 'No se pudo eliminar el director',
                                    icon: 'error'
                                });
                            }
                        },
                        error: function () {
                            Swal.fire({
                                title: 'Error',
                                text: 'Ocurrió un error al intentar eliminar el director',
                                icon: 'error'
                            });
                        }
                    });

                }


            });


        },
        cargarDirector(id) {

            $.get(`/Director/GetDirectorById?id=${id}`, function (resultado) {
                //Espacios, para dividir el proceso
                if (resultado.esCorrecto) {
                    let data = resultado.dato;

                    $('#DirectorId').val(data.directorId);
                    $('#FirstName').val(data.firstName);
                    $('#LastName').val(data.lastName);
                    $('#Nationality').val(data.nationality);
                    $('#BirthDate').val(data.birthDate);
                    $('#Biography').val(data.biography);


                    $('#modalEditarDirector').modal('show');
                }
            });
        },



    };



    $(document).ready(() => Director.init());

})(); //Encapsulamos el código para evitar conflictos con otras partes del proyecto