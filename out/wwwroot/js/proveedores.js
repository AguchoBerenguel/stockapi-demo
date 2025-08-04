const apiUrl = "https://stockapibackend-demo.onrender.com/api/proveedores"; 

window.onload = cargarProveedores;

document.getElementById("proveedor-form").onsubmit = async function (e) {
  e.preventDefault();

  const nombre = document.getElementById("nombre").value.trim();
  const telefono = document.getElementById("telefono").value.trim();
  const correo = document.getElementById("correo").value.trim();
  const direccion = document.getElementById("direccion").value.trim();

  if (!nombre || !telefono || !correo || !direccion) {
    mostrarMensaje("Completa todos los campos", "danger");
    return;
  }

  const proveedor = { nombre, telefono, correo, direccion };

  try {
    const response = await fetch(apiUrl, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(proveedor),
    });

    if (response.ok) {
      mostrarMensaje("Proveedor agregado con éxito", "success");
      document.getElementById("proveedor-form").reset();
      cargarProveedores();
    } else {
      const texto = await response.text();
      mostrarMensaje("Error al agregar proveedor: " + texto, "danger");
    }
  } catch (error) {
    console.error("Error:", error);
    mostrarMensaje("Error de conexión con la API", "danger");
  }
};

async function cargarProveedores() {
  try {
    const response = await fetch(apiUrl);
    const proveedores = await response.json();

    const tabla = document.getElementById("tabla-proveedores");
    tabla.innerHTML = "";

    proveedores.forEach((prov) => {
      const fila = document.createElement("tr");
      fila.innerHTML = `
        <td>${prov.nombre}</td>
        <td>${prov.telefono}</td>
        <td>${prov.correo}</td>
        <td>${prov.direccion}</td>
        <td>
          <button class="btn btn-danger btn-sm" onclick="eliminarProveedor(${prov.id})">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      `;
      tabla.appendChild(fila);
    });
  } catch (error) {
    console.error("Error al cargar proveedores:", error);
    mostrarMensaje("Error al cargar proveedores", "danger");
  }
}

window.eliminarProveedor = async function(id) {
  if (!confirm("¿Estás seguro de eliminar este proveedor?")) return;

  try {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: "DELETE",
    });

    if (response.ok) {
      mostrarMensaje("Proveedor eliminado", "success");
      cargarProveedores();
    } else {
      mostrarMensaje("Error al eliminar proveedor", "danger");
    }
  } catch (error) {
    console.error("Error al eliminar:", error);
    mostrarMensaje("Error de conexión al eliminar", "danger");
  }
}

function mostrarMensaje(mensaje, tipo) {
  const mensajeDiv = document.getElementById("mensaje");
  mensajeDiv.innerHTML = `<div class="alert alert-${tipo}">${mensaje}</div>`;
  setTimeout(() => {
    mensajeDiv.innerHTML = "";
  }, 3000);
}
function logout() {
    // Limpiar almacenamiento
    sessionStorage.clear();
    localStorage.clear();

    // Redireccionar forzado y evitar cache
    window.location.replace('../index.html');

    // Prevenir el uso del botón Atrás
    history.pushState(null, '', '../index.html');
    window.addEventListener('popstate', function() {
        history.go(1);
    });
}