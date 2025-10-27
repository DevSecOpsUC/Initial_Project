const AUTH_URL  = 'http://localhost:5101/api/auth/login';
const ROOMS_URL = 'http://localhost:5102/api/rooms';

const loginForm = document.getElementById("loginForm");
const loginMsg = document.getElementById("loginMessage");
const roomsSection = document.getElementById("roomsSection");
const roomsContainer = document.getElementById("roomsContainer");
const logoutBtn = document.getElementById("logoutBtn");

let token = null;

// LOGIN
loginForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  const username = document.getElementById("username").value;
  const password = document.getElementById("password").value;

  loginMsg.textContent = "Verificando...";
  
  try {
    const res = await fetch(AUTH_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password })
    });

    if (!res.ok) throw new Error("Credenciales inválidas");
    const data = await res.json();

    token = data.token;
    loginMsg.textContent = `Bienvenido ${data.user}`;
    loginForm.style.display = "none";
    roomsSection.style.display = "block";

    loadRooms();

  } catch (err) {
    loginMsg.textContent = `❌ ${err.message}`;
  }
});

// CARGAR HABITACIONES
async function loadRooms() {
  roomsContainer.innerHTML = "<p>Cargando habitaciones...</p>";
  try {
    const res = await fetch(ROOMS_URL);
    if (!res.ok) throw new Error("Error al obtener habitaciones");
    const rooms = await res.json();

    roomsContainer.innerHTML = "";
    rooms.forEach(room => {
      const div = document.createElement("div");
      div.className = "room-card";
      div.innerHTML = `
        <h3>${room.name}</h3>
        <p>💲 ${room.price.toLocaleString("es-CO")} / mes</p>
      `;
      roomsContainer.appendChild(div);
    });
  } catch (err) {
    roomsContainer.innerHTML = `<p>Error: ${err.message}</p>`;
  }
}

// LOGOUT
logoutBtn.addEventListener("click", () => {
  token = null;
  loginForm.reset();
  loginForm.style.display = "block";
  roomsSection.style.display = "none";
});
