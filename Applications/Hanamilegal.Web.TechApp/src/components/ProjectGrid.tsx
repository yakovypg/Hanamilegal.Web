import "styles/components/project-grid.css";

import React from "react";
import { useTranslation } from "react-i18next";
import type { ProjectConfig } from "types";

interface Props {
  readonly projects: ProjectConfig[];
  readonly className?: string;
}

export const ProjectGrid: React.FC<Props> = ({ projects, className }: Props) => {
  const { t } = useTranslation();

  return (
    <div className={`project-grid ${className}`} role="list">
      {projects.map((project: ProjectConfig) => {
        const name: string = t(`project.${project.translationName}.name`);
        const description: string = t(`project.${project.translationName}.description`);

        return (
          <article key={project.id} className="project-item" role="listitem">
            <div className="project-media">
              <img
                className="project-img"
                src={project.imageSource}
                alt={project.imageAlt ?? name}
              />
              <div className="project-overlay" aria-hidden="true" />
              <div className="project-caption">
                <div className="project-caption-inner">
                  <h3 className="project-name">{name}</h3>
                  <p className="project-description">{description}</p>
                </div>
              </div>
            </div>
          </article>
        );
      })}
    </div>
  );
};
