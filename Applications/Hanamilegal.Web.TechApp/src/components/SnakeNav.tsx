import "styles/components/snake-nav.css";

import {
  faEnvelope,
  faHammer,
  faHandshake,
  faStar,
  type IconDefinition
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useState } from "react";
import { useTranslation } from "react-i18next";
import { VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

interface NavigationNode {
  readonly title: string;
  readonly href: string;
  readonly icon: IconDefinition;
}

interface Props {
  readonly className?: string;
}

export const SnakeNav: React.FC = ({ className }: Props) => {
  const { t } = useTranslation();
  const [activeNavigationNodeIndex, setActiveNavigationNodeIndex] = useState<number | null>(null);

  const navigationNodes: NavigationNode[] = [
    { title: t("title.services"), href: getRoutePath(VIEW_PANELS.services), icon: faHandshake },
    { title: t("title.projects"), href: getRoutePath(VIEW_PANELS.projects), icon: faHammer },
    { title: t("title.reviews"), href: getRoutePath(VIEW_PANELS.reviews), icon: faStar },
    { title: t("title.contacts"), href: getRoutePath(VIEW_PANELS.contacts), icon: faEnvelope }
  ];

  return (
    <div>
      <nav className={`snake-nav ${className}`}>
        <div className="snake-wrapper">
          <svg
            className="snake-line"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 100 40"
            preserveAspectRatio="none"
            aria-hidden>
            <path
              d="M6 20 C22 20, 22 6, 38 6 C54 6, 54 34, 70 34 C86 34, 86 20, 94 20"
              fill="none"
              stroke="currentColor"
            />
          </svg>

          <ul className="snake-list">
            {navigationNodes.map((it: NavigationNode, idx: number) => (
              <li key={it.title} className={`snake-item pos-${idx}`}>
                <a
                  className="snake-link"
                  href={it.href}
                  onMouseEnter={() => setActiveNavigationNodeIndex(idx)}
                  onMouseLeave={() => setActiveNavigationNodeIndex(null)}
                  onFocus={() => setActiveNavigationNodeIndex(idx)}
                  onBlur={() => setActiveNavigationNodeIndex(null)}
                  onTouchStart={() => setActiveNavigationNodeIndex(idx)}>
                  <span className="snake-circle" aria-hidden>
                    <FontAwesomeIcon icon={it.icon} />
                  </span>
                </a>
              </li>
            ))}
          </ul>
        </div>
      </nav>
      <p
        className="snake-caption"
        role="status"
        aria-live="polite"
        aria-hidden={activeNavigationNodeIndex === null}>
        {activeNavigationNodeIndex !== null ? navigationNodes[activeNavigationNodeIndex].title : ""}
      </p>
    </div>
  );
};
