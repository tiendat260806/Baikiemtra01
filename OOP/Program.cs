using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsAutoSpeed
{
    public abstract class PhuongTien
    {
        private string _maPT;
        private string _tenHang;
        private int _namSanXuat;
        private decimal _giaGoc;

        public string MaPT
        {
            get { return _maPT; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _maPT = "PT000";
                else
                    _maPT = value.Trim();
            }
        }

        public string TenHang
        {
            get { return _tenHang; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Ten hang khong duoc de trong!");

                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get { return _namSanXuat; }
            set
            {
                int namHienTai = DateTime.Now.Year;

                if (value < 1900 || value > namHienTai)
                {
                    throw new ArgumentException(
                        "Nam san xuat khong hop le!");
                }

                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get { return _giaGoc; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Gia goc phai lon hon 0!");

                _giaGoc = value;
            }
        }

        public PhuongTien(
            string maPT,
            string tenHang,
            int namSanXuat,
            decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        public abstract decimal TinhGiaLanBanh();

        public virtual string GetInfo()
        {
            return $"Ma PT: {MaPT} | " +
                   $"Hang: {TenHang} | " +
                   $"Nam SX: {NamSanXuat} | " +
                   $"Gia goc: {GiaGoc:N0} VND";
        }
    }


    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get { return _soChoNgoi; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "So cho ngoi phai lon hon 0!");

                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get { return _dungTichDongCo; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Dung tich dong co phai lon hon 0!");

                _dungTichDongCo = value;
            }
        }

        public OTo(
            string maPT,
            string tenHang,
            int namSanXuat,
            decimal giaGoc,
            int soChoNgoi,
            double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                return GiaGoc
                       + GiaGoc * 0.12m
                       + GiaGoc * 0.30m;
            }
            else
            {
                return GiaGoc
                       + GiaGoc * 0.10m;
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | " +
                   $"So cho: {SoChoNgoi} | " +
                   $"Dong co: {DungTichDongCo}L | " +
                   $"Loai: O to";
        }
    }


    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get { return _dungTichXylanh; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Dung tich xylanh phai lon hon 0!");

                _dungTichXylanh = value;
            }
        }

        public XeMay(
            string maPT,
            string tenHang,
            int namSanXuat,
            decimal giaGoc,
            int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                return GiaGoc
                       + GiaGoc * 0.02m;
            }
            else
            {
                return GiaGoc
                       + GiaGoc * 0.05m;
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | " +
                   $"Phan khoi: {DungTichXylanh} cc | " +
                   $"Loai: Xe may";
        }
    }


    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _danhSach
            = new List<PhuongTien>();

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null)
            {
                Console.WriteLine(
                    "Phuong tien khong hop le!");
                return;
            }

            _danhSach.Add(pt);
        }

        public void DisplayAll()
        {
            if (_danhSach.Count == 0)
            {
                Console.WriteLine(
                    "Danh sach phuong tien trong!");
                return;
            }

            foreach (PhuongTien pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());

                Console.WriteLine(
                    $"Gia lan banh: " +
                    $"{pt.TinhGiaLanBanh():N0} VND");

                Console.WriteLine();
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0)
                return null;

            return _danhSach
                .OrderByDescending(
                    p => p.TinhGiaLanBanh())
                .FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(
            string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(p =>
                    p.TenHang.IndexOf(
                        keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;


            // TC01: Kiem tra validation nam san xuat

            Console.WriteLine("===== TC01 =====");

            try
            {
                OTo otoLoi = new OTo(
                    "OT01",
                    "Toyota",
                    1850,
                    1000000000m,
                    5,
                    2.0);

                Console.WriteLine(
                    "FAILED: Doi tuong van duoc tao.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    "PASSED: " + ex.Message);
            }


            // Tao du lieu cho cac test case

            OTo oto = new OTo(
                "OT02",
                "Toyota",
                2024,
                1000000000m,
                5,
                2.0);

            XeMay xeMay = new XeMay(
                "XM01",
                "Honda",
                2024,
                50000000m,
                150);


            // TC02: Tinh gia lan banh O To

            Console.WriteLine("\n===== TC02 =====");

            decimal giaOTo =
                oto.TinhGiaLanBanh();

            Console.WriteLine(
                $"Gia lan banh O To: " +
                $"{giaOTo:N0} VND");

            if (giaOTo == 1420000000m)
            {
                Console.WriteLine(
                    "PASSED: Khop 1,420,000,000 VND");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Ket qua khong dung.");
            }


            // TC03: Tinh gia lan banh Xe May

            Console.WriteLine("\n===== TC03 =====");

            decimal giaXeMay =
                xeMay.TinhGiaLanBanh();

            Console.WriteLine(
                $"Gia lan banh Xe May: " +
                $"{giaXeMay:N0} VND");

            if (giaXeMay == 51000000m)
            {
                Console.WriteLine(
                    "PASSED: Khop 51,000,000 VND");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Ket qua khong dung.");
            }


            // TC04: Kiem tra da hinh

            Console.WriteLine("\n===== TC04 =====");

            QuanLyPhuongTien ql =
                new QuanLyPhuongTien();

            ql.AddPhuongTien(oto);
            ql.AddPhuongTien(xeMay);

            List<PhuongTien> danhSach =
                new List<PhuongTien>();

            danhSach.Add(oto);
            danhSach.Add(xeMay);

            Console.WriteLine(
                "Goi TinhGiaLanBanh() thong qua " +
                "bien kieu PhuongTien:");

            foreach (PhuongTien pt in danhSach)
            {
                Console.WriteLine(
                    $"{pt.GetInfo()}");

                Console.WriteLine(
                    $"Gia lan banh: " +
                    $"{pt.TinhGiaLanBanh():N0} VND");
            }

            bool tc04Pass =
                danhSach[0].TinhGiaLanBanh()
                    == 1420000000m &&
                danhSach[1].TinhGiaLanBanh()
                    == 51000000m;

            if (tc04Pass)
            {
                Console.WriteLine(
                    "PASSED: Da hinh hoat dong dung.");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Da hinh hoat dong sai.");
            }

            // TC05: Tim gia lan banh cao nhat

            Console.WriteLine("\n===== TC05 =====");

            PhuongTien ptMax =
                ql.FindMaxGiaLanBanh();

            if (ptMax != null &&
                ptMax == oto &&
                ptMax.TinhGiaLanBanh() == 1420000000m)
            {
                Console.WriteLine(
                    "PASSED: Tim dung phuong tien " +
                    "co gia lan banh cao nhat.");

                Console.WriteLine(
                    ptMax.GetInfo());

                Console.WriteLine(
                    $"Gia lan banh: " +
                    $"{ptMax.TinhGiaLanBanh():N0} VND");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Khong tim dung phuong tien.");
            }


            Console.WriteLine(
                "\n===== HOAN TAT KIEM THU =====");

            Console.ReadKey();
        }
    }
}