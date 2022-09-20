import Button from '@mui/material/Button'
import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Container from '@mui/material/Container';
import styles from '../../../styles/CreateBucket.module.css'
import HoneyDrop from '../icons/honeyDrop.svg'
import Typography from '@mui/material/Typography';

export default function CreateBucket() {
    return (
        <div className={styles.createBucketContainer} >
        <div className={styles.lowerRightHoneycomb} />
        <div className={styles.upperLeftHoneyDrop} />

        </div>
    )
}