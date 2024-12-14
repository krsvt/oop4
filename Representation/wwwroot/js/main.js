// data =
// {"start":"1","persons":{"1":{"id":"1","name":"Alice","birthYear":1980,"deathYear":null,"ownUnions":["u1"],"parentUnion":null},"2":{"id":"2","name":"Bob","birthYear":1985,"deathYear":null,"ownUnions":["u1"],"parentUnion":null},"3":{"id":"3","name":"Charlie","birthYear":1990,"deathYear":null,"ownUnions":[],"parentUnion":"u1"},"4":{"id":"4","name":"Dana","birthYear":1995,"deathYear":null,"ownUnions":[],"parentUnion":"u1"}},"unions":{"u1":{"id":"u1","partner":["1","2"],"children":["3","4"]}},"links":[["1","u1"],["2","u1"],["u1","3"],["u1","4"]]}
//
//
var data;

const backend = "http://localhost:5000";
const tree = backend + "/api/tree";
const person = backend + "/api/person";
const union = backend + "/api/union";

function renderTable(persons) {
  const tableBody = document.querySelector("#personTable tbody");

  tableBody.innerHTML = "";

  persons.forEach(person => {
    const row = document.createElement("tr");

    row.innerHTML = `
          <td>${person.id}</td>
          <td>${person.name}</td>
          <td>${person.gender}</td>
          <td>${new Date(person.birthDate).toLocaleDateString()}</td>
          <td>${person.deathDate ? new Date(person.deathDate).toLocaleDateString() : "N/A"}</td>
        `;

    tableBody.appendChild(row);
  });
}

async function deleteTree() {
  const response = await fetch(tree, {
    method: 'DELETE',
  });

  if (response.status === 204) {
    alert('Tree deleted successfully!');
  } else {
    const error = await response.json();
    alert(`Failed to delete tree: ${error.detail || response.status}`);
  }
}

async function getAllPersons() {
  const response = await fetch(person);
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return await response.json();
}

async function updateTree() {
  try {
    d3.select("#tree").select("svg").remove();
    // Загрузка данных с сервера
    const response = await fetch(tree);
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    data = await response.json();

    if (!data || !data.persons || !data.unions) {
      throw new Error("Ошибка в структуре данных");
    }

    const svg = d3
      .select("#tree")
      .append("svg")
      .attr("width", document.body.offsetWidth / 2)
      .attr("height", document.documentElement.clientHeight);

    let FT = new FamilyTree(data, svg);
    FT.draw();
    renderTable(await getAllPersons());
  } catch (error) {
    console.error("Ошибка загрузки данных или рендера дерева:", error);
  }

}

async function addPerson(person) {
  try {
    const response = await fetch(person, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(person),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const result = await response.json();
    console.log("Ответ от сервера:", result);
  } catch (error) {
    console.error("Ошибка при отправке данных:", error);
  }
}

document.addEventListener("DOMContentLoaded", async () => {
  updateTree();

  const button = document.getElementById("sendButton");

  button.addEventListener("click", async () => {
    const personData = {
      name: "John Doe",
      age: 30,
      gender: "male",
    };

    addPerson(personData);
  });

  const deleteButton = document.getElementById("deleteButton");

  deleteButton.addEventListener("click", async () => {
    await deleteTree();
    await updateTree();
  });

});
