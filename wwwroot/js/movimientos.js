const productosUrl = "https://stockapibackend-demo.onrender.com/api/producto";
const movimientosUrl = "https://stockapibackend-demo.onrender.com/api/movimientos";

// Al cargar la página
window.onload = function () {
  cargarProductosEnMovimiento();
  cargarMovimientos();
  document.getElementById("movimiento-form").onsubmit = manejarFormularioMovimiento;
};

// Cargar productos en el select
async function cargarProductosEnMovimiento() {
  const res = await fetch(productosUrl, { cache: "no-store" });
  const productos = await res.json();
  const select = document.getElementById("movimiento-producto");
  if (!select) return;
  select.innerHTML = "";
  productos.forEach((p) => {
    select.innerHTML += `<option value="${p.id}">${p.nombre}</option>`;
  });
}

// Handler del formulario de movimientos
async function manejarFormularioMovimiento(e) {
  e.preventDefault();
  const ProductoId = document.getElementById("movimiento-producto").value;
  const Tipo = document.getElementById("movimiento-tipo").value;
  const Cantidad = parseInt(document.getElementById("movimiento-cantidad").value);
  const Responsable = document.getElementById("movimiento-responsable").value;
  const body = {
    productoId: ProductoId,
    tipo: Tipo,
    cantidad: Cantidad,
    responsable: Responsable
  };

  const res = await fetch(movimientosUrl, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (res.ok) {
    mostrarMensaje("Movimiento registrado.", "success");
    cargarMovimientos();
    cargarProductosEnMovimiento();
    document.getElementById("movimiento-form").reset();
  } else {
    const err = await res.text();
    mostrarMensaje("Error: " + err, "danger");
  }
}

// Cargar historial de movimientos
async function cargarMovimientos() {
  const res = await fetch(movimientosUrl, { cache: "no-store" });
  const movimientos = await res.json();
  const tbody = document.getElementById("tabla-movimientos");
  if (!tbody) return;
  tbody.innerHTML = "";

  movimientos.forEach((m) => {
    tbody.innerHTML += `
      <tr>
        <td>${new Date(m.fecha).toLocaleString()}</td>
        <td>${m.producto?.nombre || ""}</td>
        <td>${m.tipo}</td>
        <td>${m.cantidad}</td>
        <td>${m.responsable}</td>
        <td>
          <button class="btn btn-danger btn-sm" onclick="eliminarMovimiento(${m.id})">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      </tr>
    `;
  });
}

// Eliminar movimiento
window.eliminarMovimiento = async function(id) {
  if (!confirm("¿Estás seguro de eliminar este Movimiento?")) return;

  try {
    const response = await fetch(`${movimientosUrl}/${id}`, {
      method: "DELETE",
    });

    if (response.ok) {
      mostrarMensaje("Movimiento eliminado", "success");
      cargarMovimientos();
    } else {
      mostrarMensaje("Error al eliminar Movimiento", "danger");
    }
  } catch (error) {
    console.error("Error al eliminar:", error);
    mostrarMensaje("Error de conexión al eliminar", "danger");
  }
};

// Mostrar mensajes
function mostrarMensaje(texto, tipo) {
  const div = document.getElementById("mensaje");
  if (!div) return;
  div.innerHTML = `<div class="alert alert-${tipo}">${texto}</div>`;
  setTimeout(() => (div.innerHTML = ""), 2500);
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