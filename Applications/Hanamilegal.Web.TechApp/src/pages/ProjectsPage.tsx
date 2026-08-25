import { ProjectGrid } from "components";
import React from "react";
import { useTranslation } from "react-i18next";
import { PROJECT_CONFIGS, type ProjectConfig } from "types";

export const ProjectsPage: React.FC = () => {
  const { t } = useTranslation();
  const projects: ProjectConfig[] = Object.values(PROJECT_CONFIGS);

  return (
    <div>
      <h3 className="text-center text-decoration-underline">{t("title.projects")}</h3>
      <ProjectGrid className="mt-5" projects={projects} />
    </div>
  );
};
