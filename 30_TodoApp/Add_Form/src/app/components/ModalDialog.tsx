import { ReactNode } from "react";
import styles from "./ModalDialog.module.css";

type ModalDialogProps = {
  title: string;
  onOk?: () => void;
  onCancel?: () => void;
  children: ReactNode;
}

export default function ModalDialog({ title, onOk, onCancel, children }: ModalDialogProps) {
  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        <div className={styles.header}>
          <h2>{title}</h2>
        </div>
        <div className={styles.content}>
          {children}
        </div>
        <div className={styles.footer}>
          <button className={styles.cancelButton} onClick={onCancel}>
            Cancel
          </button>
          <button className={styles.okButton} onClick={onOk}>
            OK
          </button>
        </div>
      </div>
    </div>
  );
}