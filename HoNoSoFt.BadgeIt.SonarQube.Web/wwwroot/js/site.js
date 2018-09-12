var metrics = [
  "QualityGate", "Vulnerabilities", "SqaleIndex", "CodeSmells", "Coverage",
  "DuplicatedLinesDensity", "Bugs", "NewBugs", "NewVulnerabilities", "NewReliabilityRating",
  "NewSecurityRating", "Ncloc", "SqaleRating", "NewTechnicalDebt", "NewCodeSmells",
  "Tests", "SecurityRating", "NewMaintainabilityRating", "AlertStatus", "ReliabilityRating",
  "QualityGateDetails", "DuplicatedBlocks", "NclocLanguageDistribution", "NewLinesToCover"];

var uris = {
  codeSmell: "./api/badges?key={project}:{branch}&metric={metric}",
}

function updateBadges() {
  var projectName = document.getElementById("projectName").value;
  var branchName = document.getElementById("branch").value;
  var badgeContainer = document.getElementById("badgeContainer");

  if (!projectName || !branchName) {
    alert("Please provide a valid project and branch name.")
    return;
  }
  cleanBadgeContainer(badgeContainer);

  metrics.forEach(function (metricName) {
      var div = document.createElement("div");
      var img = document.createElement("img");
      img.setAttribute("src", uris.codeSmell
          .replace("{project}", projectName)
          .replace("{branch}", branchName)
          .replace("{metric}", metricName));

      div.appendChild(img);
      badgeContainer.appendChild(div);
  })
}

function cleanBadgeContainer(el) {
  while (el.firstChild) el.removeChild(el.firstChild);
}
//courrier new 48