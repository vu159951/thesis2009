using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDemo1.DTO;
using GameDemo1.Data;
using System.Windows.Forms;
using System.Drawing;

namespace GameDemo1
{
    public class MapCreator
    {
        public static String[] EDGE_MATRIX_DATA_FILE = new String[] { 
                                                            "Details.xml",
                                                            "S3.txt",
                                                            "S4.txt",
                                                            "S1.txt",
                                                            "S2.txt",
                                                            "Info.txt"};
        public static MatrixDTO MAX_VALUE_IN_COL;
        public static int CONST_FLOOR_LIMIT = 40;
        public static int CONST_CEILING_LIMIT = 38;
        public static int CONST_EPSILON = 1;

        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private static MatrixDTO S1;
        private static MatrixDTO S2;
        private static MatrixDTO S3;
        private static MatrixDTO S4;
        private static MapCellGroupCollection _mapCellGroup;


        private static int a, b;
        public static void Load(String matrixDataFolderPath)
        {
            _mapCellGroup = MapCellGroupReader.Read(
                matrixDataFolderPath + "\\" + EDGE_MATRIX_DATA_FILE[0]
                );
            S1 = MatrixMgr.ReadTextFile(
                matrixDataFolderPath + "\\" + EDGE_MATRIX_DATA_FILE[1], 
                _mapCellGroup.Quantity,
                _mapCellGroup.Quantity);
            S2 = MatrixMgr.ReadTextFile(
                            matrixDataFolderPath + "\\" + EDGE_MATRIX_DATA_FILE[2],
                            _mapCellGroup.Quantity,
                            _mapCellGroup.Quantity);
            S3 = MatrixMgr.ReadTextFile(
                matrixDataFolderPath + "\\" + EDGE_MATRIX_DATA_FILE[3],
                _mapCellGroup.Quantity,
                _mapCellGroup.Quantity);
            S4 = MatrixMgr.ReadTextFile(
                matrixDataFolderPath + "\\" + EDGE_MATRIX_DATA_FILE[4],
                _mapCellGroup.Quantity,
                _mapCellGroup.Quantity);
            MAX_VALUE_IN_COL = MatrixMgr.ReadTextFile(matrixDataFolderPath + "\\" + EDGE_MATRIX_DATA_FILE[5], _mapCellGroup.Quantity, 4);
        }
        public static MatrixDTO Generate(int width, int height)
        {
            MatrixDTO matrix = new MatrixDTO(width, height);
            for (int j = 0; j < height; j++){
                for (int i = 0; i < width; i++){
                    int[] neighbourCell = GetNeighbourCell(matrix, new Point(i, j));
                    neighbourCell[2] = -1;  // chỉ xét ô láng giềng ở hướng 1 và 2
                    neighbourCell[3] = -1;  // chỉ xét ô láng giềng ở hướng 1 và 2
                    if (GetNeighbourCellQuantity(neighbourCell) == 0)
                    {
                        // matrix.Data[i, j] = GetRandomMapCell();
                        matrix.Data[i, j] = 0;
                        // Logger.Save(i.ToString() + " " + j.ToString() + Environment.NewLine);
                    }else{
                        if (i == 2)
                            neighbourCell[1] = 46;
                        matrix.Data[i, j] = FindMapCell(neighbourCell);
                        a = i; b = j;
                    }
                }
            }

            return matrix;
        }
        public static MatrixDTO Generate(int width, int height, int[] ratio)
        {
            MatrixDTO matrix = new MatrixDTO(width, height);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    matrix.Data[i, j] = GetRandomMapCell();
                }
            }

            return matrix;
        }

        #region Private helper methods
        private static MatrixDTO GetEdgeMatrix(int index_based_zero)
        {
            switch(index_based_zero){
                case 0:
                    return S3;
                case 1:
                    return S4;
                case 2:
                    return S1;
                case 3:
                    return S2;
                default:
                    return null;
            }
        }
        private static int GetNeighbourCellQuantity(int[] neighbourCell)
        {
            if (neighbourCell.Length != 4)
                throw new Exception("Invalid parametters. Have only 4 edges.");

            int count = 0;
            for (int i = 0; i < 4; i++)
                if (neighbourCell[i] != -1)
                    count++;
            return count;
        }
        private static int[] GetNeighbourCell(MatrixDTO matrix, Point currentPos)
        {
            // Lấy giá trị 4 ô láng giềng ứng với ô ở vị trí currentPos
            int[] neighbourCell = new int[4];

            // Lấy ô 1 : left - top
            Point nCell = new Point(currentPos.X, currentPos.Y - 1);
            if (nCell.Y < 0)
                neighbourCell[0] = -1;
            else neighbourCell[0] = matrix.Data[nCell.X, nCell.Y];

            // Lấy ô 2 : right - top
            nCell = new Point(currentPos.X - 1, currentPos.Y);
            if (nCell.X < 0)
                neighbourCell[1] = -1;
            else neighbourCell[1] = matrix.Data[nCell.X, nCell.Y];

            // Lấy ô 3 : right - bottom
            nCell = new Point(currentPos.X, currentPos.Y + 1);
            if (nCell.Y >= matrix.Height)
                neighbourCell[2] = -1;
            else neighbourCell[2] = matrix.Data[nCell.X, nCell.Y];

            // Lấy ô 4 : left - bottom
            nCell = new Point(currentPos.X + 1, currentPos.Y);
            if (nCell.X >= matrix.Width)
                neighbourCell[3] = -1;
            else neighbourCell[3] = matrix.Data[nCell.X, nCell.Y];

            return neighbourCell;
        }
        private static int GetMapCellGroupId(int idMapCell)
        {
            if (idMapCell == -1)
                return -1;

            for (int i = 0; i < _mapCellGroup.Count; i++ ){
                MapCellGroup g = _mapCellGroup[i];
                if (idMapCell >= g.StartIndex && idMapCell <= g.EndIndex)
                    return g.Id;
            }
            return -1;
        }


        /// <summary>
        /// Lấy ngẫu nhiên mapcell id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetRandomMapCell()
        {
            return rnd.Next(0, _mapCellGroup.Quantity - 1);
        }
        /// <summary>
        /// Lấy ngẫu nhiên một mappcell id trong mapcell group được chỉ định
        /// </summary>
        /// <param name="groupMapCellId">Mã nhóm của các MapCells trên bản đồ</param>
        /// <returns></returns>
        private static int GetRandomMapCell(int groupMapCellId)
        {
            return rnd.Next(_mapCellGroup[groupMapCellId].StartIndex, _mapCellGroup[groupMapCellId].EndIndex);
        }
        /// <summary>
        /// Tìm một id của 1 ô thích hợp với 4 ô láng giềng xung quanh
        /// </summary>
        /// <param name="neighbourCell">
        /// Danh sách gồm 4 phần tử ứng với 4 id của 4 ô láng giềng.
        /// Nếu vị trí cạnh kề nào không có ô láng giềng thì giá trị = -1.
        /// </param>
        /// <returns>Mapcell id tìm thấy cho ô hiện tại đang xét, giá trị ô này phù hợp với 4 ô láng giềng xung quanh nó.</returns>
        private static int FindMapCell(int[] neighbourCell)
        {
            if (neighbourCell.Length != 4)
                throw new Exception("Invalid parametters. Have only 4 edges.");

            int result = -1;
            float constant = CONST_FLOOR_LIMIT;

            while (true){

                // Tìm kiếm ô thích hợp với hằng số tương ứng
                result = FindMapCell(neighbourCell, constant);

                // nếu không tìm thấy, giảm hằng số thích hợp xuống 1 đơn vị EPSILON
                if (result == -1)
                {
                    constant -= CONST_EPSILON;
                    if (constant < CONST_CEILING_LIMIT){
                        int  pos;
                        int mapGroupId = GetMapCellGroupId(neighbourCell[1]);
                        pos = 1;
                        if (mapGroupId == -1){
                            mapGroupId = GetMapCellGroupId(neighbourCell[0]);
                            pos = 0;
                        }
                        if (mapGroupId == -1){
                            mapGroupId = GetMapCellGroupId(neighbourCell[2]);
                            pos = 2;
                        }
                        if (mapGroupId == -1){
                            mapGroupId = GetMapCellGroupId(neighbourCell[3]);
                            pos = 3;
                        }
                        Logger.Save(String.Format("({0}, {1}) : {2} [{3}]", a, b, neighbourCell[pos], mapGroupId) + Environment.NewLine);
                        return GetRandomMapCell(mapGroupId);
                    }
                }else{
                    return result; // Nếu tìm thấy thì trả về kết quả
                }
            }
        }
        /// <summary>
        /// Tìm một id của 1 ô thích hợp với 4 ô láng giềng xung quanh
        /// </summary>
        /// <param name="neighbourCell">
        /// Danh sách gồm 4 phần tử ứng với 4 id của 4 ô láng giềng.
        /// Nếu vị trí cạnh kề nào không có ô láng giềng thì giá trị = -1.
        /// </param>
        /// <param name="litmitConstant">Hằng số xác định độ thích hợp</param>
        /// <returns>Mapcell id tìm thấy cho ô hiện tại đang xét, giá trị ô này phù hợp với 4 ô láng giềng xung quanh nó.</returns>
        private static int FindMapCell(int[] neighbourCell, float litmitConstant)
        {
            if (neighbourCell.Length != 4)
                throw new Exception("Invalid parametters. Have only 4 edges.");

            Dictionary<int, ValueItem> suitableList = new Dictionary<int, ValueItem>();
            //Dictionary<int, ValueItem> temp = new Dictionary<int, ValueItem>();

            int index = 0;
            int edgeIndex = 0;
            int[,] m = GetEdgeMatrix(edgeIndex).Data;
            int quantity = 0;

            // Duyệt tất
            while (true)
            {
                if (index == _mapCellGroup.Quantity - 1){
                    //Logger.Save(edgeIndex.ToString() + ": ");
                    //Logger.Save(temp);
                    //Logger.Save(Environment.NewLine);
                    //temp = new Dictionary<int, ValueItem>();
                    if (edgeIndex >= 3)
                        break;  // nếu mà đã duyệt tìm hết cả 4 ô láng giềng thì thoát khỏi vòng lặp

                    index = 0;      // thiết lập index = 0 để chuẩn bị duyệt ma trận kề tìm giá trị thích hợp cho ô láng giềng ké tiếp
                    edgeIndex++;    // chuyển sang xét ma trận kề tiếp theo
                    quantity++;
                    // Nạp ma trận ô kề vào ma trận hiện hành
                    m = GetEdgeMatrix(edgeIndex).Data;
                }

                while (edgeIndex < 4 && neighbourCell[edgeIndex] == -1){
                    // xét ô láng giềng kế tiếp 
                    // nếu giá trị của ô láng giềng kế tiếp = -1, tức là không có ô láng giềng ở hướng tượng ứng
                    // ta chuyển qua xét ô láng giềng tiếp theo đó nữa
                    ///     Sau đây là thứ tự index các ô láng giềng gần kề ô đang xét:
                    ///     0: left top
                    ///     1: right top
                    ///     2: right bottom 
                    ///     3: left bottom
                    edgeIndex++;
                }

                // Kiểm tra xem đã duyệt hết các ma trận kề chưa
                if (edgeIndex >= 4){
                    quantity--;
                    break;
                }
                // Lấy giá trị max của dòng đang xét
                int litmit = MAX_VALUE_IN_COL.Data[neighbourCell[edgeIndex], edgeIndex];
                // Tính ra định mức ở dòng tương ứng
                litmit = Convert.ToInt32((float)litmit * litmitConstant);


                int value = m[neighbourCell[edgeIndex], index]; // lấy giá trị của ma trận kề
                // nếu độ phù hợp lớn hơn ngưỡng nhất định thì đưa vào danh sách các id thích hợp

                if (value >= litmitConstant/*litmit*/){
                    if (!suitableList.ContainsKey(index)){
                        suitableList.Add(
                            index,
                            new ValueItem(index));
                    }else{
                        suitableList[index].Quantity++;
                    }

                    //temp.Add(index, new ValueItem(index));
                }
                index++;        // tăng index để xét ô kế tiếp được mô tả trong ma rận
            }

            foreach (KeyValuePair<int, ValueItem> item in suitableList){
                if (item.Value.Quantity >= quantity)
                    return item.Key;    // Tìm thấy ô thích hợp
            }
            return -1;  // không tìm thấy ô nào thích hợp với các ô láng giềng
        }
        /// <summary>
        /// Tìm một id của 1 ô trong nhóm các ô được chỉ định thích hợp với 4 ô láng giềng xung quanh
        /// </summary>
        /// <param name="neighbourCell">
        /// Danh sách gồm 4 phần tử ứng với 4 id của 4 ô láng giềng.
        /// Nếu vị trí cạnh kề nào không có ô láng giềng thì giá trị = -1.
        /// </param>
        /// <param name="groupMapCellId">Mã nhóm của các MapCells trên bản đồ</param>
        /// <returns>Mapcell id tìm thấy cho ô hiện tại đang xét, giá trị ô này phù hợp với 4 ô láng giềng xung quanh nó.</returns>
        private static int FindMapCellEx(int[] neighbourCell, int groupMapCellId)
        {
            if (neighbourCell.Length != 4)
                throw new Exception("Invalid parametters. Have only 4 edges.");

            int result = -1;
            float constant = CONST_CEILING_LIMIT;

            while (true)
            {
                // Tìm kiếm ô thích hợp với hằng số tương ứng
                result = FindMapCellEx(neighbourCell, constant, groupMapCellId);

                // nếu hằng số thích hợp đã giảm về 0 mà vẫn không tìm thấy
                if (constant == 0 && result == -1){
                    return GetRandomMapCell();      // tìm đại một ô ngẫu nhiên và trả về
                }

                // nếu không tìm thấy, giảm hằng số thích hợp xuống 1 đơn vị EPSILON
                if (result == -1){
                    constant -= CONST_EPSILON;
                    if (constant < 0)
                        constant = 0;
                }
                else return result; // Nếu tìm thấy thì trả về kết quả
            }
        }
        /// <summary>
        /// Tìm một id của 1 ô trong nhóm các ô được chỉ định thích hợp với 4 ô láng giềng xung quanh        /// </summary>
        /// <param name="neighbourCell">
        /// Danh sách gồm 4 phần tử ứng với 4 id của 4 ô láng giềng.
        /// Nếu vị trí cạnh kề nào không có ô láng giềng thì giá trị = -1.
        /// </param>
        /// <param name="litmitConstant">Hằng số xác định độ thích hợp</param>
        /// <param name="groupMapCellId">Mã nhóm của các MapCells trên bản đồ</param>
        /// <returns>Mapcell id tìm thấy cho ô hiện tại đang xét, giá trị ô này phù hợp với 4 ô láng giềng xung quanh nó.</returns>
        private static int FindMapCellEx(int[] neighbourCell, float litmitConstant, int groupMapCellId)
        {
            if (neighbourCell.Length != 4)
                throw new Exception("Invalid parametters. Have only 4 edges.");

            Dictionary<int, ValueItem> suitableList = new Dictionary<int, ValueItem>();
            int index = _mapCellGroup[groupMapCellId].StartIndex;
            int edgeIndex = 0;
            int[,] m = GetEdgeMatrix(edgeIndex).Data;

            // Duyệt tất
            while (true)
            {
                if (index == _mapCellGroup[groupMapCellId].EndIndex)
                {
                    if (edgeIndex >= 3)
                        break;  // nếu mà đã duyệt tìm hết cả 4 ô láng giềng thì thoát khỏi vòng lặp

                    index = _mapCellGroup[groupMapCellId].StartIndex;  // thiết lập index = _mapCellGroup[groupMapCellId].StartIndex để chuẩn bị duyệt ma trận kề tìm giá trị thích hợp cho ô láng giềng ké tiếp
                    edgeIndex++;    // chuyển sang xét ma trận kề tiếp theo

                    // Nạp ma trận ô kề vào ma trận hiện hành
                    m = GetEdgeMatrix(edgeIndex).Data;
                }

                while (edgeIndex < 4 && neighbourCell[edgeIndex] == -1)
                {
                    // xét ô láng giềng kế tiếp 
                    // nếu giá trị của ô láng giềng kế tiếp = -1, tức là không có ô láng giềng ở hướng tượng ứng
                    // ta chuyển qua xét ô láng giềng tiếp theo đó nữa
                    ///     Sau đây là thứ tự index các ô láng giềng gần kề ô đang xét:
                    ///     0: left top
                    ///     1: right top
                    ///     2: right bottom 
                    ///     3: left bottom
                    edgeIndex++;
                }
                if (edgeIndex >= 4)
                    break;


                int value = m[index, neighbourCell[edgeIndex]]; // lấy giá trị của ma trận kề
                // nếu độ phù hợp lớn hơn ngưỡng nhất định thì đưa vào danh sách các id thích hợp
                if (value >= litmitConstant)
                {
                    if (suitableList[index] == null)
                        suitableList.Add(
                            index,
                            new ValueItem(index));
                    else suitableList[index].Quantity++;
                }
                index++;        // tặng index để xét ô kế tiếp được mô tả trong ma rận
            }

            int quantity = GetNeighbourCellQuantity(neighbourCell);
            foreach (KeyValuePair<int, ValueItem> item in suitableList)
            {
                if (item.Value.Quantity == quantity)
                    return item.Key;    // Tìm thấy ô thích hợp
            }
            return -1;  // không tìm thấy ô nào thích hợp với các ô láng giềng
        }

        #endregion
    }
    public class ValueItem
    {
        private int _value;
        private int _quantity;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public ValueItem()
        {
            _value = -1;
            _quantity = 0;
        }
        public ValueItem(int value)
        {
            _value = value;
            _quantity = 1;
        }
        public ValueItem(int value, int quantity)
        {
            _value = value;
            _quantity = quantity;
        }
    }
}
