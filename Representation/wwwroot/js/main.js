data =
{"start":"1","persons":{"1":{"id":"1","name":"Alice","birthYear":1980,"deathYear":null,"ownUnions":["u1"],"parentUnion":null},"2":{"id":"2","name":"Bob","birthYear":1985,"deathYear":null,"ownUnions":["u1"],"parentUnion":null},"3":{"id":"3","name":"Charlie","birthYear":1990,"deathYear":null,"ownUnions":[],"parentUnion":"u1"},"4":{"id":"4","name":"Dana","birthYear":1995,"deathYear":null,"ownUnions":[],"parentUnion":"u1"}},"unions":{"u1":{"id":"u1","partner":["1","2"],"children":["3","4"]}},"links":[["1","u1"],["2","u1"],["u1","3"],["u1","4"]]}

document.addEventListener("DOMContentLoaded", async () => {
  try {
    // Загрузка данных с сервера
    const response = await fetch("http://localhost:5000/api/tree");
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const treeData = await response.json();
    console.log(treeData);

    const svg = d3
      .select("#tree")
      .append("svg")
      .attr("width", document.body.offsetWidth / 2)
      .attr("height", document.documentElement.clientHeight);

    let FT = new FamilyTree(treeData, svg);
    FT.draw();
  } catch (error) {
    console.error("Ошибка загрузки данных или рендера дерева:", error);
  }
});
