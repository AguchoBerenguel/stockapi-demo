const apiUrl = "https://stockapibackend-demo.onrender.com/api/usuarios";

document
  .getElementById("usuario-form")
  .addEventListener("submit", async function (e) {
    e.preventDefault();

    const nombreUsuario = document.getElementById("nombreUsuario").value;
    const contraseña = document.getElementById("contraseña").value;
    const rol = document.getElementById("rol").value;

    const nuevoUsuario = {
      nombreUsuario,
      contraseña,
      rol,
    };

    try {
      const response = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(nuevoUsuario),
      });

      const mensaje = document.getElementById("mensaje");

      if (response.ok) {
        mensaje.innerHTML = `<div class="alert alert-success">Usuario registrado correctamente</div>`;
        document.getElementById("usuario-form").reset();
        cargarUsuarios(); // actualizar tabla
      } else {
        mensaje.innerHTML = `<div class="alert alert-danger">Error al registrar usuario</div>`;
      }
    } catch (error) {
      console.error("Error:", error);
      document.getElementById(
        "mensaje"
      ).innerHTML = `<div class="alert alert-danger">Error al conectar con la API</div>`;
    }
  });

document.addEventListener("DOMContentLoaded", function () {
  cargarUsuarios();
});

async function cargarUsuarios() {
  try {
    const response = await fetch(apiUrl);
    const usuarios = await response.json();

    const tbody = document.getElementById("tabla-usuarios");
    tbody.innerHTML = "";

    usuarios.forEach((usuario) => {
      const fila = document.createElement("tr");
      fila.innerHTML = `
        <td>${usuario.id}</td>
        <td>${usuario.nombreUsuario}</td>
        <td>${usuario.rol}</td>
        <td>
  <div class="d-flex justify-content-center">
    <button class="btn btn-danger btn-sm" onclick="eliminarUsuario(${usuario.id})">
      <i class="bi bi-trash"></i>
    </button>
  </div>
</td>
      `;
      tbody.appendChild(fila);
    });
  } catch (error) {
    console.error("Error al cargar usuarios:", error);
  }
}
async function eliminarUsuario(id) {
  if (!confirm("¿Estás seguro de que deseas eliminar este usuario?")) return;

  try {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: "DELETE",
    });

    if (response.ok) {
      cargarUsuarios(); // recarga la tabla
    } else {
      alert("Error al eliminar usuario.");
    }
  } catch (error) {
    console.error("Error al eliminar usuario:", error);
    alert("Error al conectar con la API.");
  }
}

function logout() {
  // Limpiar almacenamiento
  sessionStorage.clear();
  localStorage.clear();

  // Redireccionar forzado y evitar cache
  window.location.replace("../index.html");

  // Prevenir el uso del botón Atrás
  history.pushState(null, "", "../index.html");
  window.addEventListener("popstate", function () {
    history.go(1);
  });
}
